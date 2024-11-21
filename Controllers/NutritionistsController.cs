using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Helpers;
using PulseFit.Management.Web.Models;
using PulseFit.Management.Web.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace PulseFit.Management.Web.Controllers
{
    public class NutritionistsController : Controller
    {
        private readonly INutritionistRepository _nutritionistRepository;
        private readonly ISpecializationRepository _specializationRepository;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly ILogger<NutritionistsController> _logger;

        public NutritionistsController(
            INutritionistRepository nutritionistRepository,
            ISpecializationRepository specializationRepository,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper,
            IUserHelper userHelper,
            IMailHelper mailHelper,
            ILogger<NutritionistsController> logger)
        {
            _nutritionistRepository = nutritionistRepository;
            _specializationRepository = specializationRepository;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var nutritionists = await _nutritionistRepository.GetAllWithUsersAsync();
            var nutritionistViewModels = nutritionists.Select(n => _converterHelper.ToNutritionistViewModel(n)).ToList();
            return View(nutritionistViewModels);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return new NotFoundViewResult("NutritionistNotFound");

            var nutritionist = await _nutritionistRepository.GetByIdWithUserAndSpecializationsAsync(id.Value);
            if (nutritionist == null) return new NotFoundViewResult("NutritionistNotFound");

            var model = _converterHelper.ToNutritionistViewModel(nutritionist);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = new NutritionistViewModel
            {
                SpecializationIds = new List<int>(),
                Specializations = await GetSpecializationSelectListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NutritionistViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 2 * 1024 * 1024) // Limite de 2 MB
                {
                    ModelState.AddModelError("ProfilePictureFile", "The file size should not exceed 2 MB.");
                    model.Specializations = await GetSpecializationSelectListAsync(); // Recarregar especializações em caso de erro
                    return View(model);
                }

                var existingUser = await _userHelper.GetUserByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "A user with this email already exists.");
                    return View(model);
                }

                try
                {
                    var user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        UserName = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        DateCreated = DateTime.UtcNow
                    };

                    if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 0)
                    {
                        user.ProfilePictureId = await _blobHelper.UploadBlobAsync(model.ProfilePictureFile, "nutritionists-pics");
                    }

                    string temporaryPassword = GenerateRandomPassword();
                    var createUserResult = await _userHelper.AddUserAsync(user, temporaryPassword);

                    if (!createUserResult.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "The user could not be created.");
                        return View(model);
                    }

                    await _userHelper.AddUserToRoleAsync(user, "Nutritionist");

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

                    var userFromDb = await _userHelper.GetUserByEmailAsync(user.Email);
                    if (userFromDb == null)
                    {
                        ModelState.AddModelError("", "User could not be found after creation.");
                        return View(model);
                    }

                    var nutritionist = new Nutritionist
                    {
                        UserId = userFromDb.Id,
                        User = userFromDb,
                        ExperienceYears = model.ExperienceYears,
                        Status = model.Status,
                        Specializations = await _specializationRepository.GetSpecializationsByIdsAsync(model.SpecializationIds)
                    };

                    await _nutritionistRepository.CreateAsync(nutritionist);

                    TempData["SuccessMessage"] = "Nutritionist created successfully and email sent with login instructions.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating nutritionist");
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                }
            }

            model.Specializations = await GetSpecializationSelectListAsync();
            return View(model);
        }

        // GET: Nutritionists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return new NotFoundViewResult("NutritionistNotFound");

            var nutritionist = await _nutritionistRepository.GetByIdWithUserAndSpecializationsAsync(id.Value);
            if (nutritionist == null) return new NotFoundViewResult("NutritionistNotFound");

            var model = _converterHelper.ToNutritionistViewModel(nutritionist);
            model.SpecializationIds = nutritionist.Specializations.Select(s => s.Id).ToList();
            model.Specializations = await GetSpecializationSelectListAsync();

            return View(model);
        }

        // POST: Nutritionists/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NutritionistViewModel model)
        {
            if (id != model.Id) return new NotFoundViewResult("NutritionistNotFound");

            if (ModelState.IsValid)
            {
                if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("ProfilePictureFile", "The file size should not exceed 2 MB.");
                    model.Specializations = await GetSpecializationSelectListAsync();
                    return View(model);
                }

                try
                {
                    var nutritionist = await _nutritionistRepository.GetByIdWithUserAndSpecializationsAsync(id);
                    if (nutritionist == null || nutritionist.User == null)
                    {
                        _logger.LogWarning("Nutritionist or associated User not found for ID: {0}", id);
                        return new NotFoundViewResult("NutritionistNotFound");
                    }

                    nutritionist.User.FirstName = model.FirstName;
                    nutritionist.User.LastName = model.LastName;
                    nutritionist.User.PhoneNumber = model.PhoneNumber;

                    if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 0)
                    {
                        nutritionist.User.ProfilePictureId = await _blobHelper.UploadBlobAsync(model.ProfilePictureFile, "nutritionists-pics");
                    }

                    var selectedSpecializations = await _specializationRepository.GetSpecializationsByIdsAsync(model.SpecializationIds);
                    nutritionist.Specializations.Clear();
                    nutritionist.Specializations.AddRange(selectedSpecializations);

                    nutritionist.ExperienceYears = model.ExperienceYears;
                    nutritionist.Status = model.Status;

                    await _nutritionistRepository.UpdateAsync(nutritionist);

                    TempData["SuccessMessage"] = "Nutritionist updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating nutritionist");
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                }
            }

            model.Specializations = await GetSpecializationSelectListAsync();
            return View(model);
        }

        // GET: Nutritionists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return new NotFoundViewResult("NutritionistNotFound");

            var nutritionist = await _nutritionistRepository.GetByIdWithUserAndSpecializationsAsync(id.Value);
            if (nutritionist == null) return new NotFoundViewResult("NutritionistNotFound");

            var model = _converterHelper.ToNutritionistViewModel(nutritionist);
            return View(model);
        }

        // POST: Nutritionists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nutritionist = await _nutritionistRepository.GetByIdWithUserAndSpecializationsAsync(id);
            if (nutritionist == null) return new NotFoundViewResult("NutritionistNotFound");

            try
            {
                if (nutritionist.Specializations != null && nutritionist.Specializations.Any())
                {
                    nutritionist.Specializations.Clear();
                }

                await _nutritionistRepository.DeleteAsync(nutritionist);

                if (nutritionist.User != null)
                {
                    await _userHelper.DeleteUserAsync(nutritionist.User);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                var errorModel = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                };

                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"Nutritionist ID {nutritionist.Id} is currently in use.";
                    ViewBag.ErrorMessage = "This nutritionist cannot be deleted due to active associations. Please remove or reassign these associations before trying again.";
                }
                else
                {
                    ViewBag.ErrorTitle = "Deletion Error";
                    ViewBag.ErrorMessage = "An unexpected error occurred during deletion. Please try again later.";
                }
                return View("Error", errorModel);
            }
        }

        private async Task<List<SpecialtyItemViewModel>> GetSpecializationSelectListAsync()
        {
            var specializations = await _specializationRepository.GetAllAsync();
            return specializations.Select(s => new SpecialtyItemViewModel
            {
                Value = s.Id.ToString(),
                Text = s.Name,
                ImageUrl = !string.IsNullOrEmpty(s.ImageName) ? $"/images/specializations/{s.ImageName}" : "/images/default-image.jpg"
            }).ToList();
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
