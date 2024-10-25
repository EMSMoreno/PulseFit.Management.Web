using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Helpers;
using PulseFit.Management.Web.Models;
using PulseFit.Management.Web.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PulseFit.Management.Web.Controllers
{
    public class PersonalTrainersController : Controller
    {
        private readonly IPersonalTrainerRepository _personalTrainerRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly ILogger<PersonalTrainersController> _logger;

        public PersonalTrainersController(
            IPersonalTrainerRepository personalTrainerRepository,
            IUserHelper userHelper,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper,
            ILogger<PersonalTrainersController> logger)
        {
            _personalTrainerRepository = personalTrainerRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
            _logger = logger;
        }

        // GET: PersonalTrainers
        public async Task<IActionResult> Index()
        {
            var personalTrainers = await _personalTrainerRepository.GetAllWithUsersAsync();
            return View(personalTrainers);
        }

        // GET: PersonalTrainers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return new NotFoundViewResult("PersonalTrainerNotFound");

            var personalTrainer = await _personalTrainerRepository.GetByIdAsync(id.Value);
            if (personalTrainer == null) return new NotFoundViewResult("PersonalTrainerNotFound");

            var model = _converterHelper.ToPersonalTrainerViewModel(personalTrainer);
            return View(model);
        }

        // GET: PersonalTrainers/Create
        public IActionResult Create()
        {
            return View(new PersonalTrainerViewModel());
        }

        // POST: PersonalTrainers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PersonalTrainerViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Verifica se o utilizador com o mesmo email já existe
                var existingUser = await _userHelper.GetUserByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "A user with this email already exists.");
                    return View(model);
                }

                try
                {
                    // Cria o utilizador associado ao PersonalTrainer com uma senha gerada automaticamente
                    var user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        UserName = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        DateCreated = DateTime.UtcNow
                    };

                    // Gera uma senha temporária ou aleatória para o utilizador
                    var randomPassword = "Temp@123";  // Podes optar por uma senha aleatória ou temporária mais segura

                    // Adiciona o utilizador ao sistema com a senha gerada
                    var result = await _userHelper.AddUserAsync(user, randomPassword);
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user could not be created.");
                        return View(model);
                    }

                    // Atribui o role "PersonalTrainer" ao utilizador
                    await _userHelper.AddUserToRoleAsync(user, "PersonalTrainer");

                    // Upload da imagem de perfil (opcional)
                    Guid imageId = Guid.Empty;
                    if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ProfilePictureFile, "personal-trainers");
                    }

                    // Converte o ViewModel em PersonalTrainer
                    var personalTrainer = await _converterHelper.ToPersonalTrainerAsync(model, imageId, true);
                    personalTrainer.UserId = user.Id;

                    await _personalTrainerRepository.CreateAsync(personalTrainer);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating personal trainer");
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                }
            }

            return View(model);
        }


        // GET: PersonalTrainers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return new NotFoundViewResult("PersonalTrainerNotFound");

            var personalTrainer = await _personalTrainerRepository.GetByIdAsync(id.Value);
            if (personalTrainer == null) return new NotFoundViewResult("PersonalTrainerNotFound");

            var model = _converterHelper.ToPersonalTrainerViewModel(personalTrainer);
            return View(model);
        }

        // POST: PersonalTrainers/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PersonalTrainerViewModel model)
        {
            if (id != model.Id) return new NotFoundViewResult("PersonalTrainerNotFound");

            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = model.ImageId;
                    if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ProfilePictureFile, "personal-trainers");
                    }

                    var personalTrainer = await _converterHelper.ToPersonalTrainerAsync(model, imageId, false);
                    await _personalTrainerRepository.UpdateAsync(personalTrainer);

                    var user = await _userHelper.GetUserByIdAsync(model.UserId);
                    if (user != null)
                    {
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.PhoneNumber = model.PhoneNumber;
                        await _userHelper.UpdateUserAsync(user);
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating personal trainer");
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                }
            }

            return View(model);
        }

        // GET: PersonalTrainers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return new NotFoundViewResult("PersonalTrainerNotFound");

            var personalTrainer = await _personalTrainerRepository.GetByIdAsync(id.Value);
            if (personalTrainer == null) return new NotFoundViewResult("PersonalTrainerNotFound");

            return View(_converterHelper.ToPersonalTrainerViewModel(personalTrainer));
        }

        // POST: PersonalTrainers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personalTrainer = await _personalTrainerRepository.GetByIdAsync(id);
            if (personalTrainer == null) return new NotFoundViewResult("PersonalTrainerNotFound");

            try
            {
                await _personalTrainerRepository.DeleteAsync(personalTrainer);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{personalTrainer.Id} is being used!";
                    ViewBag.ErrorMessage = "This personal trainer cannot be deleted. Please delete associations first.";
                }
                else
                {
                    ViewBag.ErrorTitle = "Error Deleting Personal Trainer";
                    ViewBag.ErrorMessage = "An unexpected error occurred while trying to delete this personal trainer.";
                }

                return View("Error");
            }
        }


        private async Task<bool> PersonalTrainerExists(int id)
        {
            return await _personalTrainerRepository.ExistAsync(id);
        }

        public IActionResult PersonalTrainerNotFound()
        {
            return View();
        }
    }
}
