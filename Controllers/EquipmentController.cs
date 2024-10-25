using Microsoft.AspNetCore.Mvc;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Data.Repositories;

namespace PulseFit.Management.Web.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly IEquipmentRepository _equipmentRepository;

        public EquipmentController(IEquipmentRepository equipmentRepository)
        {
            _equipmentRepository = equipmentRepository;
        }

        public async Task<IActionResult> List(int gymId)
        {
            var equipments = await _equipmentRepository.GetEquipmentsByGymIdAsync(gymId);
            return View(equipments);
        }

        public async Task<IActionResult> AddEquipment(Equipment equipment)
        {
            await _equipmentRepository.AddEquipmentAsync(equipment);
            return RedirectToAction("List", new { gymId = equipment.GymId });
        }

        public async Task<IActionResult> EditEquipment(int id)
        {
            var equipment = await _equipmentRepository.GetEquipmentByIdAsync(id);
            return View(equipment);
        }

        [HttpPost]
        public async Task<IActionResult> EditEquipment(Equipment equipment)
        {
            await _equipmentRepository.UpdateEquipmentAsync(equipment);
            return RedirectToAction("List", new { gymId = equipment.GymId });
        }

        public async Task<IActionResult> RemoveEquipment(int id)
        {
            await _equipmentRepository.RemoveEquipmentAsync(id);
            return RedirectToAction("List");
        }
    }
}
