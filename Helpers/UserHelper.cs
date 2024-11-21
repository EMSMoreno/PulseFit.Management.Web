using Microsoft.AspNetCore.Identity;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Data.Repositories;
using PulseFit.Management.Web.Models;
using System.Security.Claims;

namespace PulseFit.Management.Web.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAlertRepository _alertRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<UserHelper> _logger;

        public UserHelper(UserManager<User> userManager, SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager, IAlertRepository alertRepository, IPaymentRepository paymentRepository,IEmployeeRepository employeeRepository,
            ILogger<UserHelper> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _alertRepository = alertRepository;
            _paymentRepository = paymentRepository;
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public async Task<string> GetUserEmailByPaymentIdAsync(int paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
            {
                _logger.LogWarning("Payment not found for Payment ID: {PaymentId}", paymentId);
                return null;
            }

            var user = await _userManager.FindByIdAsync(payment.UserId);
            if (user == null)
            {
                _logger.LogWarning("User not found for User ID: {UserId}", payment.UserId);
                return null;
            }

            return user.Email;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password) =>
            await _userManager.CreateAsync(user, password);

        public async Task AddUserToRoleAsync(User user, string roleName) =>
            await _userManager.AddToRoleAsync(user, roleName);

        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword) =>
            await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

        public async Task CheckRoleAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token) =>
            await _userManager.ConfirmEmailAsync(user, token);

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user) =>
            await _userManager.GenerateEmailConfirmationTokenAsync(user);

        public async Task<string> GeneratePasswordResetTokenAsync(User user) =>
            await _userManager.GeneratePasswordResetTokenAsync(user);

        public async Task<User> GetUserByEmailAsync(string email) =>
            await _userManager.FindByEmailAsync(email);

        public async Task<User> GetUserByIdAsync(string userId) =>
            await _userManager.FindByIdAsync(userId);

        public async Task<bool> IsUserInRoleAsync(User user, string roleName) =>
            await _userManager.IsInRoleAsync(user, roleName);

        public async Task<SignInResult> LoginAsync(LoginViewModel model) =>
            await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

        public async Task LogoutAsync() =>
            await _signInManager.SignOutAsync();

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password) =>
            await _userManager.ResetPasswordAsync(user, token, password);

        public async Task<IdentityResult> UpdateUserAsync(User user) =>
            await _userManager.UpdateAsync(user);

        public async Task<SignInResult> ValidatePasswordAsync(User user, string password) =>
            await _signInManager.CheckPasswordSignInAsync(user, password, false);

        public async Task RemoveUserFromRoleAsync(User user, string roleName) =>
            await _userManager.RemoveFromRoleAsync(user, roleName);

        public async Task<List<User>> GetAllUsersInRoleAsync(string roleName) =>
            (await _userManager.GetUsersInRoleAsync(roleName)).ToList();

        public async Task<IdentityResult> DeleteUserAsync(User user) =>
    await _userManager.DeleteAsync(user);

        public async Task<string> GetUserIdByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("User Not Found!");
            }

            return user.Id;
        }

        // This method is used when you want to pass the ClaimsPrincipal (user)
        public string GetUserId(ClaimsPrincipal user)
        {
            return _userManager.GetUserId(user);  // Get the user Id from claims
        }

        // This method is used to get the UserId directly from the current logged-in user
        public string GetUserId()
        {
            var userId = _userManager.GetUserId(ClaimsPrincipal.Current);
            return userId;
        }

        public async Task<string> GetRoleAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault(); // Returns the first role associated with the user
        }

        public async Task<Employee> GetEmployeeByUserAsync(string userEmail)
        {
            var user = await GetUserByEmailAsync(userEmail);  // Search for the user by email
            if (user == null) return null;

            return await _employeeRepository.GetEmployeeByUserIdAsync(user.Id); // Search for the employee by UserId
        }

        // Optional: Uncomment if needed for specific notifications
        // public async Task NotifySecretaryPendingUserAsync(User user)
        // {
        //     var secretaryEmployees = await _employeeRepository.GetEmployeesByDepartmentAsync("Secretary");
        //     foreach (var secretaryEmployee in secretaryEmployees)
        //     {
        //         var alert = new Alert
        //         {
        //             Message = $"New User 'Pending': {user.FullName}",
        //             CreatedAt = DateTime.UtcNow,
        //             IsResolved = false,
        //             EmployeeId = secretaryEmployee.Id
        //         };
        //         await _alertRepository.CreateAsync(alert);
        //     }
        // }
    }


}
