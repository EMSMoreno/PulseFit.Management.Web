//using System;
//using System.Threading.Tasks;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using PulseFit.Management.Web.Data.Entities;
//using PulseFit.Management.Web.Helpers;
//using PulseFit.Management.Web.Models;
//using PulseFit.Management.Web.Data.Repositories;
//using Microsoft.EntityFrameworkCore;

//namespace PulseFit.Management.Web.Controllers
//{
//    public class ClientsController : Controller
//    {
//        private readonly IClientRepository _clientRepository;
//        private readonly IUserHelper _userHelper;
//        private readonly IBlobHelper _blobHelper;
//        private readonly IConverterHelper _converterHelper;
//        private readonly ISubscriptionRepository _subscriptionRepository;
//        private readonly ILogger<ClientsController> _logger;

//        public ClientsController(
//            IClientRepository clientRepository,
//            IUserHelper userHelper,
//            IBlobHelper blobHelper,
//            IConverterHelper converterHelper,
//            ISubscriptionRepository subscriptionRepository,
//            ILogger<ClientsController> logger)
//        {
//            _clientRepository = clientRepository;
//            _userHelper = userHelper;
//            _blobHelper = blobHelper;
//            _converterHelper = converterHelper;
//            _subscriptionRepository = subscriptionRepository;
//            _logger = logger;
//        }

//        // GET: Clients
//        public async Task<IActionResult> Index()
//        {
//            var clients = await _clientRepository.GetAllWithUsersAsync();
//            var clientViewModels = clients.Select(c => _converterHelper.ToClientViewModel(c)).ToList();
//            return View(clientViewModels);
//        }

//        // GET: Clients/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null) return new NotFoundViewResult("ClientNotFound");

//            var client = await _clientRepository.GetByIdWithUserAsync(id.Value);
//            if (client == null) return new NotFoundViewResult("ClientNotFound");

//            var model = _converterHelper.ToClientViewModel(client);
//            return View(model);
//        }

//        // GET: Clients/Create
//        public async Task<IActionResult> Create()
//        {
//            var model = new ClientViewModel
//            {
//                SubscriptionPlans = await _subscriptionRepository.GetSelectListAsync()
//            };
//            return View(model);
//        }

//        // POST: Clients/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(ClientViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                // Verificar tamanho da imagem
//                if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 2 * 1024 * 1024) // Limite de 2 MB
//                {
//                    ModelState.AddModelError("ProfilePictureFile", "The file size should not exceed 2 MB.");
//                    model.SubscriptionPlans = await _subscriptionRepository.GetSelectListAsync();
//                    return View(model);
//                }

//                // Verificar se o usuário com o email já existe
//                var existingUser = await _userHelper.GetUserByEmailAsync(model.Email);
//                if (existingUser != null)
//                {
//                    ModelState.AddModelError(string.Empty, "A user with this email already exists.");
//                    model.SubscriptionPlans = await _subscriptionRepository.GetSelectListAsync();
//                    return View(model);
//                }

//                try
//                {
//                    // Criação do novo User
//                    var user = new User
//                    {
//                        FirstName = model.FirstName,
//                        LastName = model.LastName,
//                        Email = model.Email,
//                        UserName = model.Email,
//                        PhoneNumber = model.PhoneNumber,
//                        DateCreated = DateTime.UtcNow
//                    };

//                    // Upload da imagem de perfil se uma imagem for enviada
//                    if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 0)
//                    {
//                        user.ProfilePictureId = await _blobHelper.UploadBlobAsync(model.ProfilePictureFile, "clients-pics");
//                    }

//                    // Gerar senha temporária
//                    string temporaryPassword = GenerateRandomPassword();
//                    var createUserResult = await _userHelper.AddUserAsync(user, temporaryPassword);

//                    if (!createUserResult.Succeeded)
//                    {
//                        ModelState.AddModelError(string.Empty, "The user could not be created.");
//                        model.SubscriptionPlans = await _subscriptionRepository.GetSelectListAsync();
//                        return View(model);
//                    }

//                    await _userHelper.AddUserToRoleAsync(user, "Client");

//                    // Criação do Client e associação ao User já salvo
//                    var client = await _converterHelper.ToClientAsync(model, user.ProfilePictureId ?? Guid.Empty, isNew: true);
//                    await _clientRepository.CreateAsync(client);

//                    TempData["SuccessMessage"] = "Client created successfully.";
//                    return RedirectToAction(nameof(Index));
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogError(ex, "Error creating client");
//                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
//                }
//            }

//            model.SubscriptionPlans = await _subscriptionRepository.GetSelectListAsync();
//            return View(model);
//        }

//        // GET: Clients/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null) return new NotFoundViewResult("ClientNotFound");

//            var client = await _clientRepository.GetByIdWithUserAsync(id.Value);
//            if (client == null) return new NotFoundViewResult("ClientNotFound");

//            var model = _converterHelper.ToClientViewModel(client);
//            model.SubscriptionPlans = await _subscriptionRepository.GetSelectListAsync();
//            return View(model);
//        }

//        // POST: Clients/Edit
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, ClientViewModel model)
//        {
//            if (id != model.Id) return new NotFoundViewResult("ClientNotFound");

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    // Verificar tamanho da imagem
//                    if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 2 * 1024 * 1024) // Limite de 2 MB
//                    {
//                        ModelState.AddModelError("ProfilePictureFile", "The file size should not exceed 2 MB.");
//                        model.SubscriptionPlans = await _subscriptionRepository.GetSelectListAsync();
//                        return View(model);
//                    }

//                    var client = await _clientRepository.GetByIdWithUserAsync(id);
//                    if (client == null || client.User == null)
//                    {
//                        _logger.LogWarning("Client or associated User not found for ID: {0}", id);
//                        return new NotFoundViewResult("ClientNotFound");
//                    }

//                    // Atualizar dados do User (exceto Email e UserName)
//                    client.User.FirstName = model.FirstName;
//                    client.User.LastName = model.LastName;
//                    client.User.PhoneNumber = model.PhoneNumber;

//                    // Atualizar a imagem do perfil se uma nova imagem for enviada
//                    if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 0)
//                    {
//                        client.User.ProfilePictureId = await _blobHelper.UploadBlobAsync(model.ProfilePictureFile, "clients-pics");
//                    }

//                    // Atualizar propriedades do Client
//                    client.Birthdate = model.Birthdate;
//                    client.Address = model.Address;
//                    client.SubscriptionPlanId = model.SubscriptionPlanId;
//                    client.Status = model.Status;
//                    client.Gender = model.Gender;

//                    await _clientRepository.UpdateAsync(client);

//                    TempData["SuccessMessage"] = "Client updated successfully.";
//                    return RedirectToAction(nameof(Index));
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogError(ex, "Error updating client");
//                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
//                }
//            }

//            model.SubscriptionPlans = await _subscriptionRepository.GetSelectListAsync();
//            return View(model);
//        }

//        // GET: Clients/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null) return new NotFoundViewResult("ClientNotFound");

//            var client = await _clientRepository.GetByIdWithUserAsync(id.Value);
//            if (client == null) return new NotFoundViewResult("ClientNotFound");

//            var model = _converterHelper.ToClientViewModel(client);
//            return View(model);
//        }

//        // POST: Clients/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var client = await _clientRepository.GetByIdWithUserAsync(id);
//            if (client == null) return new NotFoundViewResult("ClientNotFound");

//            try
//            {
//                // Remover o Client
//                await _clientRepository.DeleteAsync(client);

//                // Apagar o usuário associado ao Client
//                if (client.User != null)
//                {
//                    await _userHelper.DeleteUserAsync(client.User);
//                }

//                TempData["SuccessMessage"] = "Client deleted successfully.";
//                return RedirectToAction(nameof(Index));
//            }
//            catch (DbUpdateException ex)
//            {
//                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
//                {
//                    ViewBag.ErrorTitle = $"Client ID {client.Id} is currently in use.";
//                    ViewBag.ErrorMessage = "This client cannot be deleted due to active associations. Please remove or reassign these associations before trying again.";
//                }
//                else
//                {
//                    ViewBag.ErrorTitle = "Deletion Error";
//                    ViewBag.ErrorMessage = "An unexpected error occurred during deletion. Please try again later.";
//                }
//                return View("Error");
//            }
//        }

//        public IActionResult ClientNotFound()
//        {
//            return View();
//        }

//        private string GenerateRandomPassword(int length = 8)
//        {
//            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()?_-";
//            Random random = new Random();
//            return new string(Enumerable.Repeat(validChars, length)
//                .Select(s => s[random.Next(s.Length)]).ToArray());
//        }
//    }
//}
