using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Data.Repositories;
using PulseFit.Management.Web.Helpers;
using PulseFit.Management.Web.Models;

namespace PulseFit.Management.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConfiguration _configuration;
        private readonly IClientRepository _clientRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IUserHelper userHelper,
            IMailHelper mailHelper,
            IBlobHelper blobHelper,
            IConfiguration configuration,
            IClientRepository clientRepository,
            IConverterHelper converterHelper,
            ILogger<AccountController> logger)
        {
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _blobHelper = blobHelper;
            _configuration = configuration;
            _clientRepository = clientRepository;
            _converterHelper = converterHelper;
            _logger = logger;
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
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    // Obtém o utilizador e o respetivo role
                    var user = await _userHelper.GetUserByEmailAsync(model.Username);
                    var userRole = await _userHelper.GetRoleAsync(user);

                    // Redireciona o utilizador com base no role
                    switch (userRole)
                    {
                        case "Admin":
                            return RedirectToAction("AdminDashboard", "Dashboard");
                        case "Employee":
                            return RedirectToAction("EmployeeDashboard", "Dashboard");
                        case "PersonalTrainer":
                            return RedirectToAction("PersonalTrainerDashboard", "Dashboard");
                        case "Client":
                            return RedirectToAction("ClientDashboard", "Dashboard");
                        case "Nutritionist":
                            return RedirectToAction("NutritionistDashboard", "Dashboard");
                        default:
                            _logger.LogWarning("User {Email} has an undefined role: {Role}", model.Username, userRole);
                            return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    // Adiciona logs detalhados para cada caso de falha
                    _logger.LogWarning("Failed login attempt for user: {Email}", model.Username);

                    if (result.IsLockedOut)
                    {
                        ModelState.AddModelError("", "This account is temporarily locked.");
                    }
                    else if (result.IsNotAllowed)
                    {
                        ModelState.AddModelError("", "Login not allowed. Please confirm your email.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to log in. Please check your credentials.");
                    }
                }
            }

            return View(model);
        }


        // Logs out the user
        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        // Displays the registration page for clients
        public IActionResult Register()
        {
            var model = new RegisterNewUserViewModel();
            return View(model);
        }

        // Processes the registration for clients
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.ProfilePicture != null && model.ProfilePicture.Length > 2 * 1024 * 1024) 
                {
                    ModelState.AddModelError("ProfilePicture", "The file size should not exceed 2 MB.");
                    return View(model);
                }

                var existingUser = await _userHelper.GetUserByEmailAsync(model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already registered.");
                    return View(model);
                }

                try
                {
                    // Criação do novo User
                    var user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Username,
                        UserName = model.Username,
                        PhoneNumber = model.PhoneNumber,
                        DateCreated = DateTime.UtcNow,
                        Address = model.Address
                    };

                    if (model.ProfilePicture != null)
                    {
                        user.ProfilePictureId = await _blobHelper.UploadBlobAsync(model.ProfilePicture, "clients-pics");
                    }

                    // Adiciona o usuário
                    var createUserResult = await _userHelper.AddUserAsync(user, model.Password);
                    if (!createUserResult.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "The user could not be created.");
                        return View(model);
                    }

                    // Atribui o papel de "Client" e atualiza o estado do e-mail como confirmado
                    await _userHelper.AddUserToRoleAsync(user, "Client");
                    await _userHelper.UpdateUserAsync(user);

                    // Gera e envia o link de confirmação de email com token
                    string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    string tokenLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = myToken }, protocol: HttpContext.Request.Scheme);

                    // Load and process email template
                    string emailTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "Views/Emails/EmailConfirmationTemplate.html");
                    var placeholders = new Dictionary<string, string>
            {
                { "FirstName", user.FirstName },
                { "ConfirmationLink", tokenLink }
            };
                    string emailBody = _mailHelper.LoadAndProcessEmailTemplate(emailTemplatePath, placeholders);

                    // Send the email
                    var response = _mailHelper.SendEmail(user.Email, "Email Confirmation", emailBody);

                    if (!response.IsSuccess)
                    {
                        ModelState.AddModelError("", "Error sending confirmation email.");
                        return View(model);
                    }

                    // Cria a entidade Client associada
                    var client = new Client
                    {
                        Birthdate = model.Birthdate,
                        //Address = model.Address,
                        Gender = model.Gender,
                        UserId = user.Id,
                        Status = Status.Active,
                        RegistrationDate = DateTime.UtcNow
                    };
                    await _clientRepository.CreateAsync(client);

                    TempData["SuccessMessage"] = "Account created successfully! Check your email for further instructions.";
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating client");
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
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
            return View(new ResetPasswordViewModel { Token = token });
        }

        // Processes the password reset request
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
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
            }

            return View(model);
        }

        // GET: Account/ChangeFirstPassword
        public IActionResult ChangeFirstPassword(string email, string token)
        {
            var model = new ChangeFirstPasswordViewModel
            {
                Email = email,
                Token = token
            };
            return View(model);
        }

        // POST: Account/ChangeFirstPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeFirstPassword(ChangeFirstPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                    return View(model);
                }

                // Atualiza a senha usando o token de redefinição
                var resetPasswordResult = await _userHelper.ResetPasswordAsync(user, model.Token, model.NewPassword);
                if (resetPasswordResult.Succeeded)
                {
                    // Confirma automaticamente o email, caso ainda não esteja
                    if (!user.EmailConfirmed)
                    {
                        user.EmailConfirmed = true;
                        await _userHelper.UpdateUserAsync(user);
                    }

                    TempData["SuccessMessage"] = "Your password has been changed successfully.";
                    return RedirectToAction("Login", "Account");
                }

                foreach (var error in resetPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
    }
}
