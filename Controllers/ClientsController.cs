using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Helpers;
using PulseFit.Management.Web.Models;
using PulseFit.Management.Web.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace PulseFit.Management.Web.Controllers
{
    public class ClientsController : Controller
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IMailHelper _mailHelper;
        private readonly ILogger<ClientsController> _logger;

        public ClientsController(
            IClientRepository clientRepository,
            IUserHelper userHelper,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper,
            IMailHelper mailHelper,
            ILogger<ClientsController> logger)
        {
            _clientRepository = clientRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
            _mailHelper = mailHelper;
            _logger = logger;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            var clients = await _clientRepository.GetAllWithUsersAsync();
            var clientViewModels = clients.Select(c => _converterHelper.ToClientViewModel(c)).ToList();
            return View(clientViewModels);
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return new NotFoundViewResult("ClientNotFound");

            var client = await _clientRepository.GetByIdWithUserAsync(id.Value);
            if (client == null) return new NotFoundViewResult("ClientNotFound");

            var model = _converterHelper.ToClientViewModel(client);
            return View(model);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View(new ClientViewModel());
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Verificar tamanho da imagem
                if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 2 * 1024 * 1024) // Limite de 2 MB
                {
                    ModelState.AddModelError("ProfilePictureFile", "The file size should not exceed 2 MB.");
                    return View(model);
                }

                // Verificar se o usuário com o email já existe
                var existingUser = await _userHelper.GetUserByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "A user with this email already exists.");
                    return View(model);
                }

                try
                {
                    // Criação do novo User
                    var user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        UserName = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        DateCreated = DateTime.UtcNow
                    };

                    // Upload da imagem de perfil se uma imagem for enviada
                    if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 0)
                    {
                        user.ProfilePictureId = await _blobHelper.UploadBlobAsync(model.ProfilePictureFile, "clients-pics");
                    }

                    // Gerar senha temporária
                    string temporaryPassword = GenerateRandomPassword();
                    var createUserResult = await _userHelper.AddUserAsync(user, temporaryPassword);

                    if (!createUserResult.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "The user could not be created.");
                        return View(model);
                    }

                    // Atribuir o papel de "Client"
                    await _userHelper.AddUserToRoleAsync(user, "Client");

                    // Gera e envia o e-mail de boas-vindas
                    string token = await _userHelper.GeneratePasswordResetTokenAsync(user);
                    string resetLink = Url.Action("ChangeFirstPassword", "Account", new { email = user.Email, token = token }, protocol: HttpContext.Request.Scheme);

                    var placeholders = new Dictionary<string, string>
            {
                { "FirstName", user.FirstName },
                { "TemporaryPassword", temporaryPassword },
                { "ResetLink", resetLink }
            };

                    string emailTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "Views/Emails/WelcomeEmailTemplate.html");
                    string emailBody = _mailHelper.LoadAndProcessEmailTemplate(emailTemplatePath, placeholders);

                    var emailResponse = _mailHelper.SendEmail(user.Email, "Welcome to PulseFit", emailBody);
                    if (!emailResponse.IsSuccess)
                    {
                        ModelState.AddModelError(string.Empty, "Error sending email. Please check email settings.");
                    }

                    // Associa o User ao novo Client, utilizando o UserId do user criado
                    var client = new Client
                    {
                        Birthdate = model.Birthdate,
                        Address = model.Address,
                        Gender = model.Gender,
                        UserId = user.Id,  // Atribui o UserId ao Client
                        Status = Status.Active,
                        RegistrationDate = DateTime.UtcNow
                    };

                    // Salva o novo Client no repositório
                    await _clientRepository.CreateAsync(client);

                    TempData["SuccessMessage"] = "Client created successfully and email sent with login instructions.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating client");
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                }
            }

            return View(model);
        }



        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return new NotFoundViewResult("ClientNotFound");

            var client = await _clientRepository.GetByIdWithUserAsync(id.Value);
            if (client == null) return new NotFoundViewResult("ClientNotFound");

            var model = _converterHelper.ToClientViewModel(client);
            return View(model);
        }

        // POST: Clients/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClientViewModel model)
        {
            if (id != model.Id) return new NotFoundViewResult("ClientNotFound");

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 2 * 1024 * 1024) // Limite de 2 MB
                    {
                        ModelState.AddModelError("ProfilePictureFile", "The file size should not exceed 2 MB.");
                        return View(model);
                    }

                    var client = await _clientRepository.GetByIdWithUserAsync(id);
                    if (client == null || client.User == null)
                    {
                        _logger.LogWarning("Client or associated User not found for ID: {0}", id);
                        return new NotFoundViewResult("ClientNotFound");
                    }

                    client.User.FirstName = model.FirstName;
                    client.User.LastName = model.LastName;
                    client.User.PhoneNumber = model.PhoneNumber;

                    if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 0)
                    {
                        client.User.ProfilePictureId = await _blobHelper.UploadBlobAsync(model.ProfilePictureFile, "clients-pics");
                    }

                    client.Birthdate = model.Birthdate;
                    client.Address = model.Address;
                    client.Status = model.Status;
                    client.Gender = model.Gender;

                    await _clientRepository.UpdateAsync(client);

                    TempData["SuccessMessage"] = "Client updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating client");
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                }
            }
            return View(model);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return new NotFoundViewResult("ClientNotFound");

            var client = await _clientRepository.GetByIdWithUserAsync(id.Value);
            if (client == null) return new NotFoundViewResult("ClientNotFound");

            var model = _converterHelper.ToClientViewModel(client);
            return View(model);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _clientRepository.GetByIdWithUserAsync(id);
            if (client == null) return new NotFoundViewResult("ClientNotFound");

            try
            {
                await _clientRepository.DeleteAsync(client);

                if (client.User != null)
                {
                    await _userHelper.DeleteUserAsync(client.User);
                }

                TempData["SuccessMessage"] = "Client deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"Client ID {client.Id} is currently in use.";
                    ViewBag.ErrorMessage = "This client cannot be deleted due to active associations. Please remove or reassign these associations before trying again.";
                }
                else
                {
                    ViewBag.ErrorTitle = "Deletion Error";
                    ViewBag.ErrorMessage = "An unexpected error occurred during deletion. Please try again later.";
                }
                return View("Error");
            }
        }

        private async Task<bool> ClientExists(int id)
        {
            return await _clientRepository.ExistAsync(id);
        }

        public IActionResult ClientNotFound()
        {
            return View();
        }

        private string GenerateRandomPassword(int length = 8)
        {
            const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
            const string digitChars = "0123456789";
            const string specialChars = "!@#$%^&*()?_-";
            const int requiredUniqueChars = 1;

            Random random = new Random();

            string password = new string(new[]
            {
        upperChars[random.Next(upperChars.Length)],
        lowerChars[random.Next(lowerChars.Length)],
        digitChars[random.Next(digitChars.Length)],
        specialChars[random.Next(specialChars.Length)]
    });

            string allChars = upperChars + lowerChars + digitChars + specialChars;
            password += new string(Enumerable.Repeat(allChars, length - 4) 
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return new string(password.ToCharArray().OrderBy(_ => random.Next()).ToArray());
        }

    }
}
