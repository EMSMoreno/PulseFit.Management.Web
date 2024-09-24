using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Models;
using System.Threading.Tasks;

namespace PulseFit.Management.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Login Method
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Invalid login attempt.");
            }

            return View(model);
        }

        // Register Method
        public IActionResult Register()
        {
            var model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Criar um novo utilizador com os dados do ViewModel
                var user = new User
                {
                    UserName = model.Email, // Usado para login
                    Email = model.Email,
                    Name = model.Name,
                    BirthDate = model.BirthDate,
                    UserType = model.UserType,
                    Address = model.Address,
                    Phone = model.Phone,
                    RegistrationDate = DateTime.Now,
                    Status = "Active" // Definindo como ativo por padrão
                };

                // Criar o utilizador com o UserManager
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Adicionar o utilizador à role correspondente
                    await _userManager.AddToRoleAsync(user, model.UserType);

                    // Redirecionar para a página de login ou dashboard
                    return RedirectToAction("Login");
                }

                // Adicionar os erros de validação ao ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Se falhar, retornar a view com os erros
            return View(model);
        }

        // Logout Method
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }

        // ForgotPassword Method
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    return RedirectToAction("ForgotPasswordConfirmation");
                }

                // Generate password reset token and send email logic here
                // var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                // await _emailHelper.SendPasswordResetEmail(user.Email, token);
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // ResetPassword Method
        [HttpGet]
        public IActionResult ResetPassword(string token = null)
        {
            if (token == null)
            {
                ModelState.AddModelError("", "A token is required for password reset.");
            }

            return View(new ResetPasswordViewModel { Token = token });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }

                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}