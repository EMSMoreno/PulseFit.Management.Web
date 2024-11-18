using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Data.Repositories;
using PulseFit.Management.Web.Helpers;
using PulseFit.Management.Web.Models;

namespace PulseFit.Management.Web.Controllers
{
    public class WorkoutPlansController : Controller
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository;
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;

        public WorkoutPlansController(
            IWorkoutPlanRepository workoutPlanRepository,
            IEquipmentRepository equipmentRepository,
            IConverterHelper converterHelper,
            IBlobHelper blobHelper
            )
        {
            _workoutPlanRepository = workoutPlanRepository;
            _equipmentRepository = equipmentRepository;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
        }

        // GET: WorkoutPlans
        public IActionResult Index()
        {
            var workoutPlans = _workoutPlanRepository.GetAll().ToList()
                .GroupBy(w => w.WorkoutPlanType)
                .OrderBy(w => w.Key);

            return View(workoutPlans);
        }

        // GET: WorkoutPlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workoutPlan = await _workoutPlanRepository.GetWorkoutPlanByIdWithEquipmentsAsync(id.Value);
            if (workoutPlan == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToWorkoutPlanViewModel(workoutPlan);

            return View(model);
        }

        // GET: WorkoutPlans/Create
        public async Task<IActionResult> Create()
        {
            var model = new WorkoutPlanViewModel
            {
                EquipmentIds = new List<int>(),
                Equipments = await GetEquipmentsSelectListAsync(),
            };

            LoadViewBags();

            return View(model);
        }

        // POST: WorkoutPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkoutPlanViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.WorkoutPlanImageFile != null && model.WorkoutPlanImageFile.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("Workout Image", "The file size should not exceed 2 MB.");
                    model.Equipments = await GetEquipmentsSelectListAsync();
                    return View(model);
                }

                var imageId = model.WorkoutPlanImageFile != null
                    ? await _blobHelper.UploadBlobAsync(model.WorkoutPlanImageFile, "workoutsPlans-pics")
                    : Guid.Empty;


                var workoutPlan = await _converterHelper.ToWorkoutPlanAsync(model, imageId, true);

                await _workoutPlanRepository.CreateAsync(workoutPlan);

                return RedirectToAction(nameof(Index));
            }
            model.Equipments = await GetEquipmentsSelectListAsync();
            LoadViewBags();
            return View(model);
        }

        // GET: WorkoutPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workoutPlan = await _workoutPlanRepository.GetByIdAsync(id.Value);
            if (workoutPlan == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToWorkoutPlanViewModel(workoutPlan);

            model.EquipmentIds = workoutPlan.Equipments.Select(e => e.Id).ToList();
            model.Equipments = await GetEquipmentsSelectListAsync();

            LoadViewBags();

            return View(model);
        }

        // POST: WorkoutPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WorkoutPlanViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.WorkoutPlanImageFile != null && model.WorkoutPlanImageFile.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("Workout Image", "The file size should not exceed 2 MB.");
                    model.Equipments = await GetEquipmentsSelectListAsync();
                    return View(model);
                }

                try
                {
                    var imageId = model.WorkoutPlanImageFile != null
                    ? await _blobHelper.UploadBlobAsync(model.WorkoutPlanImageFile, "workoutsPlans-pics")
                    : Guid.Empty;

                    var workoutPlan = await _workoutPlanRepository.GetWorkoutPlanByIdWithEquipmentsAsync(model.Id);
                    var selectedEquipments = await _equipmentRepository.GetEquipmentsListByIdsAsync(model.EquipmentIds);
                    workoutPlan.Equipments.Clear();
                    workoutPlan.Equipments.AddRange(selectedEquipments);

                    await _workoutPlanRepository.UpdateAsync(workoutPlan);

                    TempData["SuccessMessage"] = "Workout Plan updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _workoutPlanRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            model.Equipments = await GetEquipmentsSelectListAsync();
            LoadViewBags();

            return View(model);
        }

        // GET: WorkoutPlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workoutPlan = await _workoutPlanRepository.GetByIdAsync(id.Value);
            if (workoutPlan == null)
            {
                return NotFound();
            }

            return View(workoutPlan);
        }

        // POST: WorkoutPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workoutPlan = await _workoutPlanRepository.GetByIdAsync(id);
            await _workoutPlanRepository.DeleteAsync(workoutPlan);

            return RedirectToAction(nameof(Index));
        }

        private async Task<List<EquipmentItemViewModel>> GetEquipmentsSelectListAsync()
        {
            var equipments = await _equipmentRepository.GetAllAsync();

            return equipments.Select(e => new EquipmentItemViewModel
            {
                Value = e.Id.ToString(),
                Text = e.Name,
                ImageUrl = e.EquipmentImageUrl,
            }).ToList();
        }


        private void LoadViewBags()
        {
            ViewBag.Type = new SelectList(Enum.GetValues(typeof(WorkoutPlan.WorkoutPlanTypeList)).Cast<WorkoutPlan.WorkoutPlanTypeList>());
            ViewBag.Difficulty = new SelectList(Enum.GetValues(typeof(WorkoutPlan.WorkoutPlanDifficulty)).Cast<WorkoutPlan.WorkoutPlanDifficulty>());
        }
    }
}
