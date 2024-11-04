using MailKit.Net.Smtp;
using MimeKit;

namespace PulseFit.Management.Web.Helpers
{
    public class MailHelper : IMailHelper
    {
        // Campo privado para armazenar a configuração da aplicação
        private readonly IConfiguration _configuration;

        // Construtor
        // Injetamos a interface IConfiguration para acessar os dados de configuração do email (armazenados em appsettings.json, por exemplo)
        public MailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Implementação do método SendEmail, que envia um email com base nas informações fornecidas
        public Response SendEmail(string to, string subject, string body)
        {
            // Recupera as configurações de email do arquivo de configuração (appsettings.json)
            var nameFrom = _configuration["Mail:NameFrom"]; // Nome do remetente, recuperado da configuração
            var from = _configuration["Mail:From"]; // Endereço de email do remetente, recuperado da configuração
            var smtp = _configuration["Mail:Smtp"]; // Endereço do servidor SMTP, recuperado da configuração
            var port = _configuration["Mail:Port"]; // Porta SMTP, recuperada da configuração
            var password = _configuration["Mail:Password"]; // Senha do email, recuperada da configuração

            // Cria uma nova mensagem de email usando MimeMessage
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(nameFrom, from)); // Adiciona o nome e endereço do remetente à mensagem
            message.To.Add(new MailboxAddress(to, to)); // Adiciona o destinatário à mensagem
            message.Subject = subject; // Define o assunto do email

            // Cria o corpo do email usando BodyBuilder
            var bodybuilder = new BodyBuilder
            {
                HtmlBody = body, // Define o corpo do email como HTML
            };
            message.Body = bodybuilder.ToMessageBody(); // Converte o corpo do email para o formato de mensagem

            try
            {
                // Usa o SmtpClient para enviar o email
                using (var client = new SmtpClient())
                {
                    client.Connect(smtp, int.Parse(port), false); // Conecta ao servidor SMTP com a porta especificada
                    client.Authenticate(from, password); // Autentica com o servidor usando o email e a senha
                    client.Send(message); // Envia a mensagem de email
                    client.Disconnect(true); // Desconecta do servidor SMTP
                }
            }
            catch (Exception ex)
            {
                // Se ocorrer uma exceção durante o envio do email, retorna uma resposta de falha
                return new Response
                {
                    IsSuccess = false, // Indica que o envio do email falhou
                    Message = ex.ToString() // Inclui detalhes da exceção na mensagem de erro
                };
            }

            // Retorna uma resposta indicando que o email foi enviado com sucesso
            return new Response
            {
                IsSuccess = true // Indica que o envio do email foi bem-sucedido
            };
        }

        public string LoadAndProcessEmailTemplate(string templatePath, Dictionary<string, string> placeholders)
        {
            // Lê o template de email a partir do caminho fornecido
            string templateContent = System.IO.File.ReadAllText(templatePath);

            // Substitui os placeholders no template
            foreach (var placeholder in placeholders)
            {
                templateContent = templateContent.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);
            }

            return templateContent;
        }


    }
}