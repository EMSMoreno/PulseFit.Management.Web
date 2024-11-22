using MailKit.Net.Smtp;
using MimeKit;

namespace PulseFit.Management.Web.Helpers
{
    public class MailHelper : IMailHelper
    {
        // Private field to store application configuration
        private readonly IConfiguration _configuration;

        // Construtor
        // Inject IConfiguration interface to access email configuration data (stored in appsettings.json, for example)
        public MailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Implementation of the SendEmail method, which sends an email based on the information provided
        public Response SendEmail(string to, string subject, string body)
        {
            // Retrieves email settings from the configuration file (appsettings.json)
            var nameFrom = _configuration["Mail:NameFrom"]; // Sender name, retrieved from configuration
            var from = _configuration["Mail:From"]; // Sender email address, retrieved from setup
            var smtp = _configuration["Mail:Smtp"]; // SMTP server address, retrieved from configuration
            var port = _configuration["Mail:Port"]; // SMTP port, retrieved from configuration
            var password = _configuration["Mail:Password"]; // Email password, retrieved from configuration

            // Create a new email message using MimeMessage
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(nameFrom, from)); // Adds the sender's name and address to the message
            message.To.Add(new MailboxAddress(to, to)); // Adding the recipient to the message
            message.Subject = subject; // Set the subject of the email

            // Create the email body using BodyBuilder
            var bodybuilder = new BodyBuilder
            {
                HtmlBody = body, // Sets the email body to HTML
            };
            message.Body = bodybuilder.ToMessageBody(); // Converts the email body to message format

            try
            {
                // Use SmtpClient to send the email
                using (var client = new SmtpClient())
                {
                    client.Connect(smtp, int.Parse(port), false); // Connects to the SMTP server with the specified port
                    client.Authenticate(from, password); // Authenticate with the server using email and password
                    client.Send(message); // Send the email message
                    client.Disconnect(true); // Disconnect from SMTP server
                }
            }
            catch (Exception ex)
            {
                // If an exception occurs while sending the email, it returns a failure response
                return new Response
                {
                    IsSuccess = false, // Indicates that sending the email failed
                    Message = ex.ToString() // Include exception details in the error message
                };
            }

            // Returns a response indicating that the email was sent successfully
            return new Response
            {
                IsSuccess = true // Indicates that the email sending was successful
            };
        }

        public string LoadAndProcessEmailTemplate(string templatePath, Dictionary<string, string> placeholders)
        {
            // Reads the email template from the provided path
            string templateContent = System.IO.File.ReadAllText(templatePath);

            // Adds current year to placeholders if not set
            if (!placeholders.ContainsKey("Year"))
            {
                placeholders["Year"] = DateTime.UtcNow.Year.ToString();
            }

            // Replace placeholders in the template
            foreach (var placeholder in placeholders)
            {
                templateContent = templateContent.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);
            }

            return templateContent;
        }

    }
}