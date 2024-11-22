using Microsoft.AspNetCore.Identity;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Models;
using System.Security.Claims;

namespace PulseFit.Management.Web.Helpers
{
    public interface IUserHelper
    {
        // Method that receives an email and returns the user corresponding to that email, if it exists.

        Task<User> GetUserByEmailAsync(string email);

        // Method that adds a new user to the system, receiving a User object and the password for registration.

        Task<IdentityResult> AddUserAsync(User user, string password);

        // Method that attempts to log in a user based on information from the LoginViewModel.

        // Returns a SignIn result that indicates whether the login was successful or not.

        Task<SignInResult> LoginAsync(LoginViewModel model);

        // Method that logs out the current user.

        Task LogoutAsync();

        // Method for updating the information of an existing user in the system.

        Task<IdentityResult> UpdateUserAsync(User user);

        // Method for changing a user's password, receiving the old password and the new password.

        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        //Rolles - Method that checks whether a certain Role exists in the system and, if it does not exist, creates it.

        Task CheckRoleAsync(string roleName);

        //Rolles - Method that adds a specific role to a user.

        Task AddUserToRoleAsync(User user, string roleName);

        //Rolles - Method that checks if a user already has a specific role assigned to them.

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        // Method that validates a user's password, returning the validation result.

        Task<SignInResult> ValidatePasswordAsync(User user, string password);

        // Method that generates a token for email confirmation and sends it to the user's email.

        // This token is used to ensure that the email provided by the user is valid.

        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        // Method that validates the email confirmation token received after the user clicks the link

        // confirmation sent to your email. If the token is valid, the user's email is confirmed.

        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        // Method that returns a User object based on the user ID.

        // This method is useful to get detailed information about a specific user using their ID.

        Task<User> GetUserByIdAsync(string userId);

        // Method that generates a token for password reset.

        // This token is sent to the user's email to allow them to reset their password.

        // Returns the generated token.

        Task<string> GeneratePasswordResetTokenAsync(User user);

        // Method that resets a user's password.

        // Receives the user, reset token and new password.

        // Returns an IdentityResult indicating whether the password reset was successful or if errors occurred.

        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);

        Task RemoveUserFromRoleAsync(User user, string roleName);

        Task<List<User>> GetAllUsersInRoleAsync(string roleName);

        Task<IdentityResult> DeleteUserAsync(User user);

        // Add method to notify the secretariat about new users "Pending"

        //Task NotifySecretaryPendingUserAsync(User user);

        Task<string> GetUserIdByEmailAsync(string email);

        Task<string> GetUserEmailByPaymentIdAsync(int paymentId);

        Task<string> GetRoleAsync(User user);

        Task<Employee> GetEmployeeByUserAsync(string userEmail);

        string GetUserId(ClaimsPrincipal user);  // Method that accepts ClaimsPrincipal

        string GetUserId();

    }
}