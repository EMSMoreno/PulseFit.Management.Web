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
        private readonly IGymRepository _gymRepository;
        private readonly IPersonalTrainerRepository _personalTrainerRepository;
        private readonly IUserHelper _userHelper;

        public WorkoutsController(
            DataContext context,
            IWorkoutRepository workoutRepository,
            IBookingRepository bookingRepository,
            IGymRepository gymRepository,
            IPersonalTrainerRepository personalTrainerRepository,
            IUserHelper userHelper
            )
        {
            _context = context;
            _workoutRepository = workoutRepository;
            _bookingRepository = bookingRepository;
            _gymRepository = gymRepository;
            _personalTrainerRepository = personalTrainerRepository;
            _userHelper = userHelper;
        }

        // GET: Workouts
        public async Task<IActionResult> Index(string searchTerm, Workout.WorkoutType? type, string instructorName, string gymName,
                                       Workout.WorkoutDifficulty? difficulty, Workout.WorkoutStatus? status,
                                       string sortOrder)
        {
            var workouts = _workoutRepository.GetAll()
                //.Include(w => w.Gym)
                //.Include(w => w.InstructorId)
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

            //if (!string.IsNullOrEmpty(instructorName))
            //{
            //    workouts = workouts.Where(w => w.Instructor.FullName.Contains(instructorName));
            //}

            //if (!string.IsNullOrEmpty(gymName))
            //{
            //    workouts = workouts.Where(w => w.Gym.Name.Contains(gymName));
            //}

            if (difficulty.HasValue)
            {
                workouts = workouts.Where(w => w.DifficultyLevel == difficulty.Value);
            }

            if (status.HasValue)
            {
                workouts = workouts.Where(w => w.Status == status.Value);
            }


            workouts = sortOrder switch
            {
                "type_desc" => workouts.OrderByDescending(w => w.Type),
                "type_asc" or "type" => workouts.OrderBy(w => w.Type),
                "duration_desc" => workouts.OrderByDescending(w => w.Duration),
                "duration_asc" or "duration" => workouts.OrderBy(w => w.Duration),
                _ => workouts.OrderBy(w => w.Name),
            };

            var workoutsList = await workouts.ToListAsync();

            foreach(var workout in workoutsList)
            {
                workout.GymName = await _gymRepository.GetGymNameByIdAsync(workout.GymId);
                workout.InstructorName = await _personalTrainerRepository.GetPtNameByIdAsync(workout.InstructorId);
            }
            return View(workoutsList);
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
        public async Task<IActionResult> Create()
        {
            ViewBag.GymId = new SelectList(_gymRepository.GetAll().Where(g => g.Status == Gym.GymStatus.Active), "Id", "Name");

            var instructors = (await _userHelper.GetAllUsersInRoleAsync("PersonalTrainer"))
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.FullName
                })
                .ToList();

            ViewBag.Instructors = instructors;


            ViewBag.Type = new SelectList(Enum.GetValues(typeof(Workout.WorkoutType)).Cast<Workout.WorkoutType>());
            ViewBag.Difficulty = new SelectList(Enum.GetValues(typeof(Workout.WorkoutDifficulty)).Cast<Workout.WorkoutDifficulty>());
            ViewBag.Status = new SelectList(Enum.GetValues(typeof(Workout.WorkoutStatus)).Cast<Workout.WorkoutStatus>());

            return View();
        }

        // POST: Workouts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Workout workout)
        {
            if (ModelState.IsValid)
            {
                workout.Bookings = 0;

                await _workoutRepository.CreateAsync(workout);

                return RedirectToAction(nameof(Index));
            }

            ViewBag.GymId = new SelectList(_gymRepository.GetAll(), "Id", "Name");

            var instructors = (await _userHelper.GetAllUsersInRoleAsync("PersonalTrainer"))
                            .Select(u => new SelectListItem
                            {
                                Value = u.Id,
                                Text = u.FullName
                            })
                            .ToList();

            ViewBag.Instructors = instructors;

            ViewBag.Type = new SelectList(Enum.GetValues(typeof(Workout.WorkoutType)).Cast<Workout.WorkoutType>());
            ViewBag.Difficulty = new SelectList(Enum.GetValues(typeof(Workout.WorkoutDifficulty)).Cast<Workout.WorkoutDifficulty>());
            ViewBag.Status = new SelectList(Enum.GetValues(typeof(Workout.WorkoutStatus)).Cast<Workout.WorkoutStatus>());

            return View(workout);
        }

        // GET: Workouts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workout = await _workoutRepository.GetByIdAsync(id.Value);
            if (workout == null)
            {
                return NotFound();
            }

            ViewData["GymId"] = new SelectList(_context.Gyms, "Id", "Id", workout.GymId);
            ViewData["InstructorId"] = new SelectList(_context.Users, "Id", "Id", workout.InstructorId);

            ViewBag.Type = new SelectList(Enum.GetValues(typeof(Workout.WorkoutType)).Cast<Workout.WorkoutType>());
            ViewBag.Difficulty = new SelectList(Enum.GetValues(typeof(Workout.WorkoutDifficulty)).Cast<Workout.WorkoutDifficulty>());
            ViewBag.Status = new SelectList(Enum.GetValues(typeof(Workout.WorkoutStatus)).Cast<Workout.WorkoutStatus>());

            return View(workout);
        }

        // POST: Workouts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Workout workout)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _workoutRepository.UpdateAsync(workout);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _workoutRepository.ExistAsync(workout.Id))
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

            ViewBag.Type = new SelectList(Enum.GetValues(typeof(Workout.WorkoutType)).Cast<Workout.WorkoutType>());
            ViewBag.Difficulty = new SelectList(Enum.GetValues(typeof(Workout.WorkoutDifficulty)).Cast<Workout.WorkoutDifficulty>());
            ViewBag.Status = new SelectList(Enum.GetValues(typeof(Workout.WorkoutStatus)).Cast<Workout.WorkoutStatus>());

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
                //.Include(w => w.Gym)
                //.Include(w => w.Instructor)
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
            var workout = await _workoutRepository.GetByIdAsync(id);
            await _workoutRepository.DeleteAsync(workout);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MyBookings()
        {
            var userLoged = User.Identity.Name;
            if (userLoged != null)
            {
                string userId = await _userHelper.GetUserIdByEmailAsync(userLoged);

                var bookings = await _bookingRepository.GetBookingsByUserAsync(userId);

                return View(bookings);
            }

            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> CreateBooking(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userLoged = User.Identity.Name;
            if (userLoged != null)
            {
                string userId = await _userHelper.GetUserIdByEmailAsync(userLoged);

                //var maximumCapacity = await _bookingRepository.WorkoutMaximumCapacityReachedAsync(workoutId);

                var workout = await _workoutRepository.GetByIdAsync(id.Value);

                bool maximumCapacityReached = true;

                if (workout.MaxCapacity == workout.Bookings)
                {
                    maximumCapacityReached = true;
                }
                else
                {
                    maximumCapacityReached = false;
                }


                if (maximumCapacityReached == false)
                {
                    try
                    {
                        var booking = new Booking
                        {
                            WorkoutId = id.Value,
                            UserId = userId,
                            TrainingDate = workout.StartDate,
                            GymId = workout.GymId
                        };

                        await _bookingRepository.CreateBookingAsync(booking);

                        workout.Bookings += 1;
                        await _workoutRepository.UpdateAsync(workout);
                        TempData["SuccessMessage"] = "Booking Confirmed.";
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorMessage"] = ex.Message;
                    }
                    return RedirectToAction("MyBookings");
                }

                ModelState.AddModelError(string.Empty, "Workout Maximum Capacity Reached!");

            }

            return RedirectToAction("Login", "Account");
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