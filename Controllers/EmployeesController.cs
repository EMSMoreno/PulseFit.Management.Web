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
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace PulseFit.Management.Web.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(
            IEmployeeRepository employeeRepository,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper,
            IUserHelper userHelper,
            IMailHelper mailHelper,
            ILogger<EmployeesController> logger)
        {
            _employeeRepository = employeeRepository;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _logger = logger;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeRepository.GetAllWithUsersAsync();
            var employeeViewModels = employees.Select(e => _converterHelper.ToEmployeeViewModel(e)).ToList();
            return View(employeeViewModels);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return new NotFoundViewResult("EmployeeNotFound");

            var employee = await _employeeRepository.GetByIdWithUserAsync(id.Value);
            if (employee == null) return new NotFoundViewResult("EmployeeNotFound");

            var model = _converterHelper.ToEmployeeViewModel(employee);
            return View(model);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View(new EmployeeViewModel());
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel model)
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
                        user.ProfilePictureId = await _blobHelper.UploadBlobAsync(model.ProfilePictureFile, "employees-pics");
                    }

                    // Gerar senha temporária
                    string temporaryPassword = GenerateRandomPassword();
                    var createUserResult = await _userHelper.AddUserAsync(user, temporaryPassword);

                    if (!createUserResult.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "The user could not be created.");
                        return View(model);
                    }

                    await _userHelper.AddUserToRoleAsync(user, "Employee");

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

                    // Verificar se o usuário foi salvo antes de associá-lo ao Employee
                    var userFromDb = await _userHelper.GetUserByEmailAsync(user.Email);
                    if (userFromDb == null)
                    {
                        ModelState.AddModelError("", "User could not be found after creation.");
                        return View(model);
                    }

                    // Criação do Employee e associação ao User já salvo
                    var employee = new Employee
                    {
                        EmployeeType = model.EmployeeType,
                        HireDate = model.HireDate,
                        Status = model.Status,
                        Shift = model.Shift,
                        UserId = userFromDb.Id,
                        User = userFromDb
                    };

                    await _employeeRepository.CreateAsync(employee);

                    TempData["SuccessMessage"] = "Employee created successfully and email sent with login instructions.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating employee");
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                }
            }

            return View(model);
        }



        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return new NotFoundViewResult("EmployeeNotFound");

            var employee = await _employeeRepository.GetByIdWithUserAsync(id.Value);
            if (employee == null) return new NotFoundViewResult("EmployeeNotFound");

            var model = _converterHelper.ToEmployeeViewModel(employee);
            return View(model);
        }

        // POST: Employees/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeViewModel model)
        {
            if (id != model.Id) return new NotFoundViewResult("EmployeeNotFound");

            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar tamanho da imagem
                    if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 2 * 1024 * 1024) // Limite de 2 MB
                    {
                        ModelState.AddModelError("ProfilePictureFile", "The file size should not exceed 2 MB.");
                        return View(model);
                    }

                    var employee = await _employeeRepository.GetByIdWithUserAsync(id);
                    if (employee == null || employee.User == null)
                    {
                        _logger.LogWarning("Employee or associated User not found for ID: {0}", id);
                        return new NotFoundViewResult("EmployeeNotFound");
                    }

                    // Atualizar dados do User (exceto Email e UserName)
                    employee.User.FirstName = model.FirstName;
                    employee.User.LastName = model.LastName;
                    employee.User.PhoneNumber = model.PhoneNumber;

                    // Atualizar a imagem do perfil se uma nova imagem for enviada
                    if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 0)
                    {
                        employee.User.ProfilePictureId = await _blobHelper.UploadBlobAsync(model.ProfilePictureFile, "employees-pics");
                    }

                    // Atualiza o Employee específico com enum Shift e HireDate opcional
                    employee.EmployeeType = model.EmployeeType;
                    employee.HireDate = model.HireDate;  // Aceita nulo
                    employee.Shift = model.Shift;        // Enum ShiftType
                    employee.Status = model.Status;

                    await _employeeRepository.UpdateAsync(employee);

                    TempData["SuccessMessage"] = "Employee updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating employee");
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                }
            }
            return View(model);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return new NotFoundViewResult("EmployeeNotFound");

            var employee = await _employeeRepository.GetByIdWithUserAsync(id.Value);
            if (employee == null) return new NotFoundViewResult("EmployeeNotFound");

            var model = _converterHelper.ToEmployeeViewModel(employee);
            return View(model);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _employeeRepository.GetByIdWithUserAsync(id);
            if (employee == null) return new NotFoundViewResult("EmployeeNotFound");

            try
            {
   

                // Remover o Employee
                await _employeeRepository.DeleteAsync(employee);

                // Apagar o usuário associado ao Employee
                if (employee.User != null)
                {
                    await _userHelper.DeleteUserAsync(employee.User);
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
                    ViewBag.ErrorTitle = $"Employee ID {employee.Id} is currently in use.";
                    ViewBag.ErrorMessage = "This employee cannot be deleted due to active associations. Please remove or reassign these associations before trying again.";
                }
                else
                {
                    ViewBag.ErrorTitle = "Deletion Error";
                    ViewBag.ErrorMessage = "An unexpected error occurred during deletion. Please try again later.";
                }
                return View("Error", errorModel);
            }
        }






        private async Task<bool> EmployeeExists(int id)
        {
            return await _employeeRepository.ExistAsync(id);
        }

        public IActionResult EmployeeNotFound()
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
