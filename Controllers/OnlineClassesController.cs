using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Data;
using PulseFit.Management.Web.Data.Repositories;
using PulseFit.Management.Web.Helpers;
using PulseFit.Management.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PulseFit.Management.Web.Controllers
{
    public class OnlineClassesController : Controller
    {
        private readonly IOnlineClassRepository _onlineClassRepository;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public OnlineClassesController(
            IOnlineClassRepository onlineClassRepository,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper
            )
        {
            _onlineClassRepository = onlineClassRepository;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }

        // GET: OnlineClasses
        public async Task<IActionResult> Index()
        {
            var onlineClasses = _onlineClassRepository.GetAll();

            return View(onlineClasses);
        }

        // GET: OnlineClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onlineClass = await _onlineClassRepository.GetByIdAsync(id.Value);
            if (onlineClass == null)
            {
                return NotFound();
            }

            return View(onlineClass);
        }

        // GET: OnlineClasses/Create
        public IActionResult Create()
        {
            ViewBag.Category = new SelectList(Enum.GetValues(typeof(OnlineClass.ClassCategory)).Cast<OnlineClass.ClassCategory>());

            return View();
        }

        // POST: OnlineClasses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OnlineClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                var imageId = model.ClassImageFile != null
                    ? await _blobHelper.UploadBlobAsync(model.ClassImageFile, "onlineClasses-pics")
                    : Guid.Empty;

                var onlineClass = _converterHelper.ToOnlineClass(model, imageId, true);

                await _onlineClassRepository.CreateAsync(onlineClass);

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Category = new SelectList(Enum.GetValues(typeof(OnlineClass.ClassCategory)).Cast<OnlineClass.ClassCategory>());

            return View(model);
        }

        // GET: OnlineClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onlineClass = await _onlineClassRepository.GetByIdAsync(id.Value);
            if (onlineClass == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToOnlineClassViewModel(onlineClass);
            ViewBag.Category = new SelectList(Enum.GetValues(typeof(OnlineClass.ClassCategory)).Cast<OnlineClass.ClassCategory>());

            return View(onlineClass);
        }

        // POST: OnlineClasses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OnlineClassViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var imageId = model.ClassImageFile != null
                    ? await _blobHelper.UploadBlobAsync(model.ClassImageFile, "onlineClasses-pics")
                    : Guid.Empty;

                    var onlineClass = _converterHelper.ToOnlineClass(model, imageId, false);

                    await _onlineClassRepository.UpdateAsync(onlineClass);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _onlineClassRepository.ExistAsync(model.Id))
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

            ViewBag.Category = new SelectList(Enum.GetValues(typeof(OnlineClass.ClassCategory)).Cast<OnlineClass.ClassCategory>());

            return View(model);
        }

        // GET: OnlineClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var onlineClass = await _onlineClassRepository.GetByIdAsync(id.Value);
            if (onlineClass == null)
            {
                return NotFound();
            }

            return View(onlineClass);
        }

        // POST: OnlineClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var onlineClass = await _onlineClassRepository.GetByIdAsync(id);
            await _onlineClassRepository.DeleteAsync(onlineClass);

            return RedirectToAction(nameof(Index));
        }
    }
}
