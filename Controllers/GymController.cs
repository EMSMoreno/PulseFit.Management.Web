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
    public class GymsController : Controller
    {
        private readonly IGymRepository _gymRepository;
        private readonly DataContext _context;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public GymsController(IGymRepository gymRepository, DataContext context, IBlobHelper blobHelper, IConverterHelper converterHelper)
        {
            _gymRepository = gymRepository;
            _context = context;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }

        // GET: Gyms
        public IActionResult Index()
        {
            return View(_gymRepository.GetAll());
        }

        // GET: Gyms/Details/5
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
        public IActionResult Create()
        {
            ViewBag.Status = new SelectList(Enum.GetValues(typeof(Gym.GymStatus)).Cast<Gym.GymStatus>());
            ViewBag.DayOff = new SelectList(Enum.GetValues(typeof(Gym.GymDayOff)).Cast<Gym.GymDayOff>());

            return View();
        }


        // POST: Gyms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GymViewModel model)
        {
            if (ModelState.IsValid)
            {
                var imageId = model.GymImageFile != null
                    ? await _blobHelper.UploadBlobAsync(model.GymImageFile, "gyms-pics")
                    : Guid.Empty;

                var gym = await _converterHelper.ToGym(model, imageId, isNew : true);

                await _gymRepository.CreateAsync(gym);
                
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Status = new SelectList(Enum.GetValues(typeof(Gym.GymStatus)).Cast<Gym.GymStatus>());
            ViewBag.DayOff = new SelectList(Enum.GetValues(typeof(Gym.GymDayOff)).Cast<Gym.GymDayOff>());

            return View(model);
        }

        // GET: Gyms/Edit/5
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

            ViewBag.Status = new SelectList(Enum.GetValues(typeof(Gym.GymStatus)).Cast<Gym.GymStatus>());
            ViewBag.DayOff = new SelectList(Enum.GetValues(typeof(Gym.GymDayOff)).Cast<Gym.GymDayOff>());

            return View(gym);
        }

        // POST: Gyms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Gym gym)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _gymRepository.UpdateAsync(gym);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _gymRepository.ExistAsync(gym.Id))
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

            ViewBag.Status = new SelectList(Enum.GetValues(typeof(Gym.GymStatus)).Cast<Gym.GymStatus>());
            ViewBag.DayOff = new SelectList(Enum.GetValues(typeof(Gym.GymDayOff)).Cast<Gym.GymDayOff>());

            return View(gym);
        }

        // GET: Gyms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gym = await _context.Gyms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gym == null)
            {
                return NotFound();
            }

            return View(gym);
        }

        // POST: Gyms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gym = await _gymRepository.GetByIdAsync(id);
            
            await _gymRepository.DeleteAsync(gym);

            return RedirectToAction(nameof(Index));
        }
    }
}
