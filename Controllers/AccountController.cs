using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Helpers;
using PulseFit.Management.Web.Models;

namespace PulseFit.Management.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IConfiguration _configuration;

        public AccountController(
            IUserHelper userHelper,
            IMailHelper mailHelper,
            IConfiguration configuration)
        {
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _configuration = configuration;
        }

        // Displays the login page
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Processes the login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);

                if (result.Succeeded)
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    }
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Failed to log in.");
            return View(model);
        }

        // Logs out the user
        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        // Displays the registration page
        public IActionResult Register()
        {
            var model = new RegisterNewUserViewModel();
            return View(model);
        }

        // Processes the registration
        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);

                if (user == null)
                {
                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Username,
                        UserName = model.Username,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                        DateCreated = DateTime.UtcNow
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user could not be created.");
                        return View(model);
                    }

                    // Automatically assign the role "Pending" to the new user
                    await _userHelper.AddUserToRoleAsync(user, "Pending");


                    // Notifica os funcionários da secretaria sobre o novo utilizador "Pending"
                    //await _userHelper.NotifySecretaryPendingUserAsync(user);

                    // Email confirmation logic (optional)
                    string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    string tokenLink = Url.Action("ConfirmEmail", "Account", new { userid = user.Id, token = myToken }, protocol: HttpContext.Request.Scheme);

                    Helpers.Response response = _mailHelper.SendEmail(model.Username, "Email Confirmation",
                        $"<h1>Email Confirmation</h1>To confirm your email, click this link: <a href=\"{tokenLink}\">Confirm Email</a>");

                    if (response.IsSuccess)
                    {
                        ViewBag.Message = "Instructions to confirm your user have been sent to your email.";
                        return View(model);
                    }

                    ModelState.AddModelError(string.Empty, "Error sending confirmation email.");
                }
            }

            return View(model);
        }

        // Displays the change user details page
        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

            if (user == null)
            {
                return RedirectToAction("NotAuthorized");
            }

            var model = new ChangeUserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }

        // Processes the change user details request
        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Address = model.Address;
                    user.PhoneNumber = model.PhoneNumber;

                    // Save changes to the user
                    var result = await _userHelper.UpdateUserAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError(string.Empty, "Failed to update user details.");
                }
                else
                {
                    return RedirectToAction("NotAuthorized");
                }
            }

            return View(model);
        }

        // Displays the not authorized page
        public IActionResult NotAuthorized()
        {
            return View();
        }

        // Email confirmation
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return View("Error");
            }

            return RedirectToAction("Login");
        }

        // Displays the change password page
        public IActionResult ChangePassword()
        {
            return View();
        }

        // Processes the change password request
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ChangeUser");
                    }
                    ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return View(model);
        }

        // Displays the password recovery page
        public IActionResult RecoverPassword()
        {
            return View();
        }

        // Processes the password recovery request
        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email does not correspond to a registered user.");
                    return View(model);
                }

                var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
                var link = Url.Action("ResetPassword", "Account", new { token = myToken }, protocol: HttpContext.Request.Scheme);

                Helpers.Response response = _mailHelper.SendEmail(model.Email, "Password Reset", $"<h1>Password Reset</h1>Click here to reset your password: <a href=\"{link}\">Reset Password</a>");

                if (response.IsSuccess)
                {
                    ViewBag.Message = "Instructions to recover your password have been sent to your email.";
                }

                return View();
            }

            return View(model);
        }

        // Displays the password reset page
        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        // Processes the password reset request
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.UserName);
            if (user != null)
            {
                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    ViewBag.Message = "Password reset successfully.";
                    return View();
                }

                ViewBag.Message = "Error resetting the password.";
                return View(model);
            }

            ViewBag.Message = "User not found.";
            return View(model);
        }
    }
}

