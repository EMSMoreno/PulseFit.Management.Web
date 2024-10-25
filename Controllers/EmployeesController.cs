using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Helpers;
using PulseFit.Management.Web.Models;
using PulseFit.Management.Web.Data.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace PulseFit.Management.Web.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(
            IEmployeeRepository employeeRepository,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper,
            IUserHelper userHelper,
            ILogger<EmployeesController> logger)
        {
            _employeeRepository = employeeRepository;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
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

            var employee = await _employeeRepository.GetByIdAsync(id.Value);
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
                // Verifica se o utilizador com o mesmo email já existe
                var existingUser = await _userHelper.GetUserByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "A user with this email already exists.");
                    return View(model);
                }

                try
                {
                    // Cria o utilizador associado ao Employee com uma senha gerada automaticamente
                    var user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        UserName = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        DateCreated = DateTime.UtcNow
                    };

                    // Gera uma senha aleatória para o utilizador
                    var randomPassword = "Temp@123";  // Aqui podes usar uma senha mais segura ou aleatória

                    // Adiciona o utilizador ao sistema com a senha gerada
                    var result = await _userHelper.AddUserAsync(user, randomPassword);
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user could not be created.");
                        return View(model);
                    }

                    // Atribui o role "Employee" ao utilizador
                    await _userHelper.AddUserToRoleAsync(user, "Employee");

                    // Upload da imagem de perfil (opcional)
                    Guid imageId = Guid.Empty;
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "employees-pics");
                    }

                    // Converte o ViewModel em Employee
                    var employee = await _converterHelper.ToEmployeeAsync(model, imageId, true);
                    employee.UserId = user.Id;

                    await _employeeRepository.CreateAsync(employee);

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

            var employee = await _employeeRepository.GetByIdAsync(id.Value);
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
                    Guid imageId = model.ImageId;

                    // Atualiza a imagem se disponível
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "employees-pics");
                    }

                    // Atualiza o Employee
                    var employee = await _converterHelper.ToEmployeeAsync(model, imageId, false);
                    await _employeeRepository.UpdateAsync(employee);

                    // Atualiza os dados do User associados
                    var user = await _userHelper.GetUserByIdAsync(model.UserId);
                    if (user != null)
                    {
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.PhoneNumber = model.PhoneNumber;
                        await _userHelper.UpdateUserAsync(user);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Associated user not found.");
                        return View(model);
                    }

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

            var employee = await _employeeRepository.GetByIdAsync(id.Value);
            if (employee == null) return new NotFoundViewResult("EmployeeNotFound");

            var model = _converterHelper.ToEmployeeViewModel(employee);
            return View(model);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return new NotFoundViewResult("EmployeeNotFound");

            try
            {
                await _employeeRepository.DeleteAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
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
                return View("Error");
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
    }
}
