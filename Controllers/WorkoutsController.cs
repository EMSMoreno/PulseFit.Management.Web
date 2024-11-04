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

namespace PulseFit.Management.Web.Controllers
{
    public class WorkoutsController : Controller
    {
        private readonly DataContext _context;
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserHelper _userHelper;

        public WorkoutsController(
            DataContext context, 
            IWorkoutRepository workoutRepository,
            IBookingRepository bookingRepository,
            IUserHelper userHelper
            )
        {
            _context = context;
            _workoutRepository = workoutRepository;
            _bookingRepository = bookingRepository;
            _userHelper = userHelper;
        }

        // GET: Workouts
        public async Task<IActionResult> Index(string searchTerm, Workout.WorkoutType? type, string instructorName, string gymName,
                                       Workout.WorkoutDifficulty? difficulty, Workout.WorkoutStatus? status,
                                       string sortOrder)
        {
            var workouts = _workoutRepository.GetAll()
                .Include(w => w.Gym)
                .Include(w => w.Instructor)
                .AsQueryable();

            // Filtro de pesquisa simples
            if (!string.IsNullOrEmpty(searchTerm))
            {
                workouts = workouts.Where(w => w.Name.Contains(searchTerm) || w.Description.Contains(searchTerm));
            }

            if (type.HasValue)
            {
                workouts = workouts.Where(w => w.Type == type.Value);
            }

            if (!string.IsNullOrEmpty(instructorName))
            {
                workouts = workouts.Where(w => w.Instructor.FullName.Contains(instructorName));
            }

            if (!string.IsNullOrEmpty(gymName))
            {
                workouts = workouts.Where(w => w.Gym.Name.Contains(gymName));
            }

            if (difficulty.HasValue)
            {
                workouts = workouts.Where(w => w.DifficultyLevel == difficulty.Value);
            }

            if (status.HasValue)
            {
                workouts = workouts.Where(w => w.Status == status.Value);
            }

            // Ordenação baseada no sortOrder
            workouts = sortOrder switch
            {
                "type_desc" => workouts.OrderByDescending(w => w.Type),
                "type_asc" or "type" => workouts.OrderBy(w => w.Type),
                "duration_desc" => workouts.OrderByDescending(w => w.Duration),
                "duration_asc" or "duration" => workouts.OrderBy(w => w.Duration),
                _ => workouts.OrderBy(w => w.Name), // Ordenação padrão
            };

            return View(await workouts.ToListAsync());
        }



        // GET: Workouts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workout = await _workoutRepository.GetByIdAsync(id.Value);

            //TODO : verificar se passa o gym e o instrutor
            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

        // GET: Workouts/Create
        public IActionResult Create()
        {
            ViewData["GymId"] = new SelectList(_context.Gyms, "Id", "Id");
            ViewData["InstructorId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Workouts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Duration,Type,Popularity,DifficultyLevel,StartDate,EndDate,MaxCapacity,Status,InstructorId,GymId")] Workout workout)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workout);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GymId"] = new SelectList(_context.Gyms, "Id", "Id", workout.GymId);
            ViewData["InstructorId"] = new SelectList(_context.Users, "Id", "Id", workout.InstructorId);
            return View(workout);
        }

        // GET: Workouts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workout = await _context.Workouts.FindAsync(id);
            if (workout == null)
            {
                return NotFound();
            }
            ViewData["GymId"] = new SelectList(_context.Gyms, "Id", "Id", workout.GymId);
            ViewData["InstructorId"] = new SelectList(_context.Users, "Id", "Id", workout.InstructorId);
            return View(workout);
        }

        // POST: Workouts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Duration,Type,Popularity,DifficultyLevel,StartDate,EndDate,MaxCapacity,Status,InstructorId,GymId")] Workout workout)
        {
            if (id != workout.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workout);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkoutExists(workout.Id))
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
            ViewData["GymId"] = new SelectList(_context.Gyms, "Id", "Id", workout.GymId);
            ViewData["InstructorId"] = new SelectList(_context.Users, "Id", "Id", workout.InstructorId);
            return View(workout);
        }

        // GET: Workouts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workout = await _context.Workouts
                .Include(w => w.Gym)
                .Include(w => w.Instructor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

        // POST: Workouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workout = await _context.Workouts.FindAsync(id);
            if (workout != null)
            {
                _context.Workouts.Remove(workout);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkoutExists(int id)
        {
            return _context.Workouts.Any(e => e.Id == id);
        }


        public async Task<IActionResult> MyBookings()
        {
            var userLoged = User.Identity.Name;
            int userId = await _userHelper.GetUserIdByEmailAsync(userLoged);

            var bookings = await _bookingRepository.GetBookingsByUserAsync(userId);

            return View(bookings);
        }

        public async Task<IActionResult> CreateBooking(int workoutId, DateTime trainingDate)
        {
            try
            {
                var userLoged = User.Identity.Name;
                int userId = await _userHelper.GetUserIdByEmailAsync(userLoged);

                var booking = new Booking
                {
                    WorkoutId = workoutId,
                    UserId = userId,
                    TrainingDate = trainingDate,
                };

                await _bookingRepository.CreateBookingAsync(booking);
                TempData["SuccessMessage"] = "Booking Confirmed.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("MyBookings");
        }

        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            try
            {
                await _bookingRepository.CancelBookingAsync(bookingId);
                TempData["SuccessMessage"] = "Booking Cancelled Successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("MyBookings");
        }
    }
}
