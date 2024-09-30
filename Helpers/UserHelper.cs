using Microsoft.AspNetCore.Identity;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Data.Repositories;
using PulseFit.Management.Web.Helpers;
using PulseFit.Management.Web.Models;

namespace PulseFit.Management.Web.Helpers
{
    public class UserHelper : IUserHelper

    {   //Classe que faz a gestao dos utilizadores. Como é uma classe do .NetCore nao necessitamos de injetar no Startup.cs

        // Classe UserManager que gere todas as operações relacionadas com os utilizadores.

        // Inclui funcionalidades como a criação, atualização, eliminação e gestão de papéis dos utilizadores.

        private readonly UserManager<User> _userManager;

        //Classe que faz a gestao dos SignIn's.Como é uma classe do .NetCore nao necessitamos de injetar no Startup.cs

        // Classe SignInManager que gere as operações de início de sessão dos utilizadores.

        // Inclui funcionalidades como autenticação e encerramento de sessão.

        private readonly SignInManager<User> _signInManager;

        // Classe RoleManager que gere os papéis (roles) dos utilizadores.

        // Inclui funcionalidades para criar, eliminar e verificar papéis.

        private readonly RoleManager<IdentityRole> _roleManager;

        //private readonly IEmployeeRepository _employeeRepository;

        private readonly IAlertRepository _alertRepository;

        // Construtor que recebe as instâncias do UserManager, SignInManager e RoleManager 

        // e as atribui às propriedades privadas correspondentes.

        //Ctrl  + . em cima do userManager e clicar em "Create and assign field userManager"

        //Ctrl  + . em cima do signInManager e clicar em "Create and assign field signInManager"

        public UserHelper(

            UserManager<User> userManager,

            SignInManager<User> signInManager,

            //Ctrl  + . em cima do roleManager e clicar em "Create and assign field roleManager"

            RoleManager<IdentityRole> roleManager,

            //IEmployeeRepository employeeRepository,

            IAlertRepository alertRepository)

        {

            _userManager = userManager;       // Inicializa o UserManager

            _signInManager = signInManager;   // Inicializa o SignInManager

            _roleManager = roleManager;       // Inicializa o RoleManager

            //_employeeRepository = employeeRepository;

            _alertRepository = alertRepository;

        }

        // Método assíncrono para criar um novo utilizador com uma palavra-passe específica.

        // Retorna um IdentityResult que indica se a operação foi bem-sucedida ou não.

        public async Task<IdentityResult> AddUserAsync(User user, string password)

        {

            return await _userManager.CreateAsync(user, password);

        }

        //Roles - Método assíncrono para adicionar um papel (role) específico a um utilizador.

        // Este método não retorna um valor, pois assume-se que a operação é bem-sucedida.

        public async Task AddUserToRoleAsync(User user, string roleName)

        {

            await _userManager.AddToRoleAsync(user, roleName);

        }

        // Método assíncrono para alterar a palavra-passe de um utilizador.

        // Requer a palavra-passe antiga e a nova palavra-passe, e retorna um IdentityResult

        // indicando se a operação foi bem-sucedida ou não.

        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)

        {

            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

        }

        //Roles - Método assíncrono para verificar se um determinado papel (role) existe no sistema.

        // Se o papel não existir, ele é criado. Isto é útil para garantir que certos papéis essenciais

        // estejam sempre disponíveis na aplicação.

        public async Task CheckRoleAsync(string roleName)

        {

            // Verifica se o papel já existe.

            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            // Se o papel não existir, cria-o.

            if (!roleExists)

            {

                await _roleManager.CreateAsync(new IdentityRole

                {

                    Name = roleName  // Define o nome do novo papel.

                });

            }

        }

        // Método assíncrono para confirmar o email de um utilizador com base num token de confirmação.

        // Retorna um IdentityResult que indica se a operação foi bem-sucedida ou não.

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)

        {

            return await _userManager.ConfirmEmailAsync(user, token);

        }

        // Método assíncrono para gerar um token de confirmação de email para um utilizador.

        // Este token é normalmente enviado por email para que o utilizador possa confirmar o seu endereço de email.

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)

        {

            return await _userManager.GenerateEmailConfirmationTokenAsync(user);

        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)

        {

            return await _userManager.GeneratePasswordResetTokenAsync(user);

        }

        // Método assíncrono para obter um utilizador pelo endereço de email.

        // Retorna o objeto User correspondente ao email fornecido, ou null se o utilizador não for encontrado.

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        // Método assíncrono para obter um utilizador pelo seu ID.

        // Retorna o objeto User correspondente ao ID fornecido, ou null se o utilizador não for encontrado.

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        //Roles - Método assíncrono para verificar se um utilizador pertence a um determinado papel (role).

        // Retorna um valor booleano indicando se o utilizador está ou não no papel especificado.

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        // Método assíncrono para realizar o login de um utilizador.

        // Recebe um objeto LoginViewModel com as credenciais e opções de sessão, e tenta autenticar o utilizador.

        // Retorna um SignInResult que indica se a operação foi bem-sucedida ou se falhou.

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(

                model.Username,   // Nome de utilizador fornecido

                model.Password,   // Palavra-passe fornecida

                model.RememberMe, // Indica se a sessão deve ser lembrada

                false);           // Indica se deve bloquear o utilizador após uma falha de login
        }

        // Método assíncrono para terminar a sessão de um utilizador.

        // Simplesmente chama o método SignOutAsync do SignInManager.

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }

        // Método assíncrono para atualizar as informações de um utilizador.

        // Recebe o objeto User com as novas informações e tenta atualizar na base de dados.

        // Retorna um IdentityResult indicando se a operação foi bem-sucedida ou não.

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
           return await _userManager.UpdateAsync(user);
        }

        // Método assíncrono para validar a palavra-passe de um utilizador.

        // Verifica se a palavra-passe fornecida é correta para o utilizador especificado.

        // Retorna um SignInResult indicando se a palavra-passe é válida ou não.

        public async Task<SignInResult> ValidatePasswordAsync(User user, string password)
        {
            return await _signInManager.CheckPasswordSignInAsync(

                user,        // Utilizador a validar

                password,    // Palavra-passe fornecida

                false);      // Indica se deve bloquear o utilizador após uma falha de validação
        }

        public async Task RemoveUserFromRoleAsync(User user, string roleName)
        {
            await _userManager.RemoveFromRoleAsync(user, roleName);
        }

        public async Task<List<User>> GetAllUsersInRoleAsync(string roleName)
        {
            IList<User> usersInRole = await _userManager.GetUsersInRoleAsync(roleName);

            return usersInRole.ToList();
        }

        //public async Task NotifySecretaryPendingUserAsync(User user)
        //{
        //    var secretaryEmployees = await _employeeRepository.GetEmployeesByDepartmentAsync("Secretary");

        //    foreach (var secretaryEmployee in secretaryEmployees)

        //    {
        //        var alert = new Alert

        //        {

        //            Message = $"New User 'Pending': {user.FullName}",

        //            CreatedAt = DateTime.UtcNow,

        //            IsResolved = false,

        //            EmployeeId = secretaryEmployee.Id

        //        };

        //        await _alertRepository.CreateAsync(alert);

        //    }

        }
    }


