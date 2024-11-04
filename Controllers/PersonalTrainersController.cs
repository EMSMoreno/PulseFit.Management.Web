﻿using System;
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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PulseFit.Management.Web.Controllers
{
    public class PersonalTrainersController : Controller
    {
        private readonly IPersonalTrainerRepository _personalTrainerRepository;
        private readonly ISpecialtyRepository _specialtyRepository;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly ILogger<PersonalTrainersController> _logger;

        public PersonalTrainersController(
            IPersonalTrainerRepository personalTrainerRepository,
            ISpecialtyRepository specialtyRepository,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper,
            IUserHelper userHelper,
            IMailHelper mailHelper,
            ILogger<PersonalTrainersController> logger)
        {
            _personalTrainerRepository = personalTrainerRepository;
            _specialtyRepository = specialtyRepository;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var trainers = await _personalTrainerRepository.GetAllWithUsersAsync();
            var trainerViewModels = trainers.Select(t => _converterHelper.ToPersonalTrainerViewModel(t)).ToList();
            return View(trainerViewModels);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return new NotFoundViewResult("TrainerNotFound");

            var trainer = await _personalTrainerRepository.GetByIdWithUserAndSpecialtiesAsync(id.Value);
            if (trainer == null) return new NotFoundViewResult("TrainerNotFound");

            var model = _converterHelper.ToPersonalTrainerViewModel(trainer);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = new PersonalTrainerViewModel
            {
                SpecialtyIds = new List<int>(),
                Specialties = await GetSpecialtySelectListAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PersonalTrainerViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Verificar tamanho da imagem
                if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 2 * 1024 * 1024) // Limite de 2 MB
                {
                    ModelState.AddModelError("ProfilePictureFile", "The file size should not exceed 2 MB.");
                    model.Specialties = await GetSpecialtySelectListAsync(); // Recarregar especialidades em caso de erro
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
                        user.ProfilePictureId = await _blobHelper.UploadBlobAsync(model.ProfilePictureFile, "personaltrainers-pics");
                    }

                    // Gerar senha temporária
                    string temporaryPassword = GenerateRandomPassword();
                    var createUserResult = await _userHelper.AddUserAsync(user, temporaryPassword);


                    if (!createUserResult.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "The user could not be created.");
                        return View(model);
                    }

                    await _userHelper.AddUserToRoleAsync(user, "PersonalTrainer");

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

                    // Verificar se o usuário foi salvo antes de associá-lo ao Personal Trainer
                    var userFromDb = await _userHelper.GetUserByEmailAsync(user.Email);
                    if (userFromDb == null)
                    {
                        ModelState.AddModelError("", "User could not be found after creation.");
                        return View(model);
                    }

                    // Criação do Personal Trainer e associação ao User já salvo
                    var trainer = new PersonalTrainer
                    {
                        UserId = userFromDb.Id,
                        User = userFromDb,
                        Certification = model.Certification,
                        HireDate = model.HireDate,
                        Status = model.Status,
                        Specialties = await _specialtyRepository.GetSpecialtiesByIdsAsync(model.SpecialtyIds)
                    };

                    await _personalTrainerRepository.CreateAsync(trainer);

                    TempData["SuccessMessage"] = "Personal Trainer created successfully and email sent with login instructions.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating personal trainer");
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                }
            }

            model.Specialties = await GetSpecialtySelectListAsync();
            return View(model);
        }


        // GET: PersonalTrainers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return new NotFoundViewResult("TrainerNotFound");

            var trainer = await _personalTrainerRepository.GetByIdWithUserAndSpecialtiesAsync(id.Value);
            if (trainer == null) return new NotFoundViewResult("TrainerNotFound");

            var model = _converterHelper.ToPersonalTrainerViewModel(trainer);
            model.SpecialtyIds = trainer.Specialties.Select(s => s.Id).ToList();
            model.Specialties = await GetSpecialtySelectListAsync();

            return View(model);
        }

        // POST: PersonalTrainers/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PersonalTrainerViewModel model)
        {
            if (id != model.Id) return new NotFoundViewResult("TrainerNotFound");

            if (ModelState.IsValid)
            {
                // Verificar tamanho da imagem
                if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 2 * 1024 * 1024) // Limite de 2 MB
                {
                    ModelState.AddModelError("ProfilePictureFile", "The file size should not exceed 2 MB.");
                    model.Specialties = await GetSpecialtySelectListAsync(); // Recarregar especialidades em caso de erro
                    return View(model);
                }

                try
                {
                    var trainer = await _personalTrainerRepository.GetByIdWithUserAndSpecialtiesAsync(id);
                    if (trainer == null || trainer.User == null)
                    {
                        _logger.LogWarning("Trainer or associated User not found for ID: {0}", id);
                        return new NotFoundViewResult("TrainerNotFound");
                    }

                    // Atualizar dados do User sem modificar Email e UserName
                    trainer.User.FirstName = model.FirstName;
                    trainer.User.LastName = model.LastName;
                    trainer.User.PhoneNumber = model.PhoneNumber;

                    // Atualizar a imagem do perfil se uma nova imagem for enviada
                    if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 0)
                    {
                        trainer.User.ProfilePictureId = await _blobHelper.UploadBlobAsync(model.ProfilePictureFile, "personaltrainers-pics");
                    }

                    // Atualizar especialidades
                    var selectedSpecialties = await _specialtyRepository.GetSpecialtiesByIdsAsync(model.SpecialtyIds);
                    trainer.Specialties.Clear();
                    trainer.Specialties.AddRange(selectedSpecialties);

                    // Atualizar os campos específicos do Personal Trainer
                    trainer.Certification = model.Certification;
                    trainer.HireDate = model.HireDate;
                    trainer.Status = model.Status;

                    await _personalTrainerRepository.UpdateAsync(trainer);

                    TempData["SuccessMessage"] = "Personal Trainer updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating personal trainer");
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                }
            }

            model.Specialties = await GetSpecialtySelectListAsync();
            return View(model);
        }

        // GET: PersonalTrainers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return new NotFoundViewResult("TrainerNotFound");

            var trainer = await _personalTrainerRepository.GetByIdWithUserAndSpecialtiesAsync(id.Value);
            if (trainer == null) return new NotFoundViewResult("TrainerNotFound");

            var model = _converterHelper.ToPersonalTrainerViewModel(trainer);
            return View(model);
        }

        // POST: PersonalTrainers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainer = await _personalTrainerRepository.GetByIdWithUserAndSpecialtiesAsync(id);
            if (trainer == null) return new NotFoundViewResult("TrainerNotFound");

            try
            {
                // Apaga todas as associações do Personal Trainer, como Specialties e Clients, se necessário
                if (trainer.Specialties != null && trainer.Specialties.Any())
                {
                    trainer.Specialties.Clear();
                }

                if (trainer.Clients != null && trainer.Clients.Any())
                {
                    trainer.Clients.Clear();
                }

                // Remover o Personal Trainer
                await _personalTrainerRepository.DeleteAsync(trainer);

                // Apagar o usuário associado ao Personal Trainer
                if (trainer.User != null)
                {
                    await _userHelper.DeleteUserAsync(trainer.User);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"Trainer ID {trainer.Id} is currently in use.";
                    ViewBag.ErrorMessage = "This trainer cannot be deleted due to active associations. Please remove or reassign these associations before trying again.";
                }
                else
                {
                    ViewBag.ErrorTitle = "Deletion Error";
                    ViewBag.ErrorMessage = "An unexpected error occurred during deletion. Please try again later.";
                }
                return View("Error");
            }
        }



        private async Task<List<SpecialtyItemViewModel>> GetSpecialtySelectListAsync()
        {
            var specialties = await _specialtyRepository.GetAllAsync();
            return specialties.Select(s => new SpecialtyItemViewModel
            {
                Value = s.Id.ToString(),
                Text = s.Name,
                ImageUrl = !string.IsNullOrEmpty(s.ImageName) ? $"/images/specialties/{s.ImageName}" : "/images/default-image.jpg"
            }).ToList();
        }


        private string GenerateRandomPassword(int length = 8)
        {
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()?_-";
            Random random = new Random();
            return new string(Enumerable.Repeat(validChars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
