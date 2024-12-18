﻿using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class GymsController : Controller
    {
        private readonly IGymRepository _gymRepository;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public GymsController(IGymRepository gymRepository, IBlobHelper blobHelper, IConverterHelper converterHelper)
        {
            _gymRepository = gymRepository;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }

        // GET: Gyms
        [Authorize]
        public IActionResult Index()
        {
            var gyms = _gymRepository.GetAll();

            return View(gyms);
        }

        // GET: Gyms/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gym = await _gymRepository.GetByIdAsync(id.Value);
            if (gym == null)
            {
                return NotFound();
            }

            return View(gym);
        }

        // GET: Gyms/Create
        [Authorize(Roles = "Admin, Employee")]
        public IActionResult Create()
        {
            return View();
        }


        // POST: Gyms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Create(GymViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.GymImageFile != null && model.GymImageFile.Length > 2 * 1024 * 1024) 
                {
                    ModelState.AddModelError("GymImageFile", "The file size should not exceed 2 MB.");
                    return View(model);
                }

                var imageId = model.GymImageFile != null
                    ? await _blobHelper.UploadBlobAsync(model.GymImageFile, "gyms-pics")
                    : Guid.Empty;


                model.CreationDate = DateTime.Now.Date;

                var gym = _converterHelper.ToGym(model, imageId, isNew : true);

                await _gymRepository.CreateAsync(gym);
                
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Gyms/Edit/5
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gym = await _gymRepository.GetByIdAsync(id.Value);
            if (gym == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToGymViewModel(gym);

            return View(model);
        }

        // POST: Gyms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Edit(GymViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.GymImageFile != null && model.GymImageFile.Length > 2 * 1024 * 1024) 
                {
                    ModelState.AddModelError("GymImageFile", "The file size should not exceed 2 MB.");
                    return View(model);
                }

                try
                {
                    Guid imageId;
                    if (model.GymImageFile != null)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.GymImageFile, "gyms-pics");
                    }
                    else
                    {
                        imageId = model.GymImageId;
                    }

                    var gym = _converterHelper.ToGym(model, imageId, false);

                    await _gymRepository.UpdateAsync(gym);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _gymRepository.ExistAsync(model.Id))
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

            return View(model);
        }

        // GET: Gyms/Delete/5
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gym = await _gymRepository.GetByIdAsync(id.Value);
            if (gym == null)
            {
                return NotFound();
            }

            return View(gym);
        }

        // POST: Gyms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gym = await _gymRepository.GetByIdAsync(id);
            await _gymRepository.DeleteAsync(gym);

            return RedirectToAction(nameof(Index));
        }
    }
}
