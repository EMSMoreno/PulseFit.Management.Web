using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Data.Repositories;
using PulseFit.Management.Web.Helpers;
using PulseFit.Management.Web.Models;

namespace PulseFit.Management.Web.Controllers
{
    public class EquipmentsController : Controller
    {
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IGymRepository _gymRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;

        public EquipmentsController(
            IEquipmentRepository equipmentRepository,
            IGymRepository gymRepository,
            IConverterHelper converterHelper,
            IBlobHelper blobHelper
            )
        {
            _equipmentRepository = equipmentRepository;
            _gymRepository = gymRepository;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
        }

        // GET: Equipments
        public IActionResult Index()
        {
            var equipments = _equipmentRepository.GetAll().ToList() // Carrega todos os equipamentos na memória
                .GroupBy(e => e.Type) // Agrupa os equipamentos por tipo em memória
                .OrderBy(g => g.Key); // Ordena os grupos por tipo

            return View(equipments);
        }



        // GET: Equipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _equipmentRepository.GetByIdAsync(id.Value);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // GET: Equipments/Create
        public IActionResult Create()
        {
            LoadViewBags();

            return View();
        }

        // POST: Equipments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EquimentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var imageId = model.EquipmentImageFile != null
                    ?await _blobHelper.UploadBlobAsync(model.EquipmentImageFile, "equipments-pics")
                    : Guid.Empty;


                model.GymName = await _gymRepository.GetGymNameByIdAsync(model.GymId);

                var equipment = _converterHelper.ToEquipment(model, imageId, true);

                await _equipmentRepository.CreateAsync(equipment);

                return RedirectToAction(nameof(Index));
            }

            LoadViewBags();

            return View(model);
        }

        // GET: Equipments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _equipmentRepository.GetByIdAsync(id.Value);
            if (equipment == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToEquipmentViewModel(equipment);

            LoadViewBags();

            return View(model);
        }

        // POST: Equipments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EquimentViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var imageId = model.EquipmentImageFile != null
                    ? await _blobHelper.UploadBlobAsync(model.EquipmentImageFile, "equipments-pics")
                    : model.EquipmentImageId;

                    model.GymName = await _gymRepository.GetGymNameByIdAsync(model.GymId);

                    var equipment = _converterHelper.ToEquipment(model, imageId, false);

                    await _equipmentRepository.UpdateAsync(equipment);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _equipmentRepository.ExistAsync(model.Id))
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

            LoadViewBags();

            return View(model);
        }

        // GET: Equipments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _equipmentRepository.GetByIdAsync(id.Value);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // POST: Equipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipment = await _equipmentRepository.GetByIdAsync(id);
            await _equipmentRepository.DeleteAsync(equipment);

            return RedirectToAction(nameof(Index));
        }

        private void LoadViewBags()
        {
            ViewBag.Gyms = new SelectList(_gymRepository.GetAll().Where(g => g.Status == Gym.GymStatus.Active), "Id", "Name");
            ViewBag.Type = new SelectList(Enum.GetValues(typeof(Equipment.EquipmentType)).Cast<Equipment.EquipmentType>());
            ViewBag.Status = new SelectList(Enum.GetValues(typeof(Equipment.EquipmentStatus)).Cast<Equipment.EquipmentStatus>());
        }
    }
}
