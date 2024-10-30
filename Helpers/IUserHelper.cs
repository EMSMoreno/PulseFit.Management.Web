using Microsoft.AspNetCore.Identity;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Models;
 
namespace PulseFit.Management.Web.Helpers
{
    public interface IUserHelper
    {
        // Método que recebe um email e devolve o utilizador correspondente a esse email, caso ele exista.

        Task<User> GetUserByEmailAsync(string email);

        // Método que adiciona um novo utilizador ao sistema, recebendo um objeto User e a senha para o registo.

        Task<IdentityResult> AddUserAsync(User user, string password);

        // Método que tenta fazer o login de um utilizador com base nas informações de LoginViewModel.

        // Devolve um resultado de SignIn que indica se o login foi bem-sucedido ou não.

        Task<SignInResult> LoginAsync(LoginViewModel model);

        // Método que realiza o logout do utilizador atual.

        Task LogoutAsync();

        // Método para atualizar as informações de um utilizador existente no sistema.

        Task<IdentityResult> UpdateUserAsync(User user);

        // Método para alterar a senha de um utilizador, recebendo a senha antiga e a nova senha.

        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        //Rolles - Método que verifica se um determinado Role (papel) existe no sistema e, se não existir, cria-o.

        Task CheckRoleAsync(string roleName);

        //Rolles - Método que adiciona um papel específico a um utilizador.

        Task AddUserToRoleAsync(User user, string roleName);

        //Rolles - Método que verifica se um utilizador já possui um papel específico atribuído.

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        // Método que valida a senha de um utilizador, devolvendo o resultado da validação.

        Task<SignInResult> ValidatePasswordAsync(User user, string password);

        // Método que gera um token para confirmação de email e o envia para o email do utilizador.

        // Esse token é utilizado para garantir que o email fornecido pelo utilizador é válido.

        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        // Método que valida o token de confirmação de email recebido após o usuário clicar no link

        // de confirmação enviado para o seu email. Se o token for válido, o email do utilizador é confirmado.

        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        // Método que retorna um objeto User baseado no ID do utilizador.

        // Esse método é útil para obter informações detalhadas de um utilizador específico usando seu ID.

        Task<User> GetUserByIdAsync(string userId);

        // Método que gera um token para redefinição de senha.

        // Este token é enviado para o email do utilizador para permitir que ele redefina a sua senha.

        // Retorna o token gerado.

        Task<string> GeneratePasswordResetTokenAsync(User user);

        // Método que redefine a senha de um utilizador.

        // Recebe o utilizador, o token de redefinição e a nova senha.

        // Retorna um IdentityResult indicando se a redefinição da senha foi bem-sucedida ou se ocorreram erros.

        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);

        Task RemoveUserFromRoleAsync(User user, string roleName);

        Task<List<User>> GetAllUsersInRoleAsync(string roleName);

        // Adiciona o método para notificar a secretaria sobre novos utilizadores "Pending"

        //Task NotifySecretaryPendingUserAsync(User user);


        Task<int> GetUserIdByEmailAsync(string email);
    }

}

