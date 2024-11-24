using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data;
using PulseFit.Management.Web.Data.Entities;
using PulseFit.Management.Web.Data.Repositories;
using PulseFit.Management.Web.Helpers;
using PulseFit.Management.Web.Models;

namespace PulseFit.Management.Web.Controllers
{
    [Authorize]
    public class WorkoutsController : Controller
    {
        private readonly DataContext _context;
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IGymRepository _gymRepository;
        private readonly IPersonalTrainerRepository _personalTrainerRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public WorkoutsController(
            DataContext context,
            IWorkoutRepository workoutRepository,
            IBookingRepository bookingRepository,
            IGymRepository gymRepository,
            IPersonalTrainerRepository personalTrainerRepository,
            IUserHelper userHelper,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper
            )
        {
            _context = context;
            _workoutRepository = workoutRepository;
            _bookingRepository = bookingRepository;
            _gymRepository = gymRepository;
            _personalTrainerRepository = personalTrainerRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }

        // GET: Workouts
        public IActionResult Index()
        {
            var workouts = _workoutRepository.GetAll();

            var combinedTypes = Enum.GetValues(typeof(Workout.IndividualWorkoutType))
                .Cast<Workout.IndividualWorkoutType>()
                .Select(type => new { Value = type.ToString(), Text = type.ToString() })
                .Concat(
                    Enum.GetValues(typeof(Workout.GroupWorkoutType))
                    .Cast<Workout.GroupWorkoutType>()
                    .Select(type => new { Value = type.ToString(), Text = type.ToString() })
                )
                .ToList();

            ViewBag.TypeOptions = new SelectList(combinedTypes, "Value", "Text");

            return View(workouts);
        }

        // GET: Workouts/Details/5
        public async Task<IActionResult> Details(int? id)
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

            ViewBag.Spots = workout.MaxCapacity - workout.Bookings;
            ViewBag.GymImage = await _gymRepository.GetGymImageAsync(workout.GymId);
            ViewBag.PtProfilePic = await _userHelper.GetUserPicAsync(workout.InstructorId);


            return View(workout);
        }

        // GET: Workouts/Create
        [Authorize(Roles = "Admin, Employee, PersonalTrainer")]
        public async Task<IActionResult> Create()
        {

            await LoadViewBags();

            return View();
        }

        // POST: Workouts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Employee, PersonalTrainer")]
        public async Task<IActionResult> Create(WorkoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.WorkoutImageFile != null && model.WorkoutImageFile.Length > 2 * 1024 * 1024) 
                {
                    ModelState.AddModelError("WorkoutImageFile", "The file size should not exceed 2 MB.");
                    return View(model);
                }

                var imageId = model.WorkoutImageFile != null
                    ? await _blobHelper.UploadBlobAsync(model.WorkoutImageFile, "workouts-pics")
                    : Guid.Empty;

                if (model.IndividualType == null && model.GroupType == null)
                {
                    ModelState.AddModelError(string.Empty, "You must select the Workout Type!");

                    await LoadViewBags();

                    return View(model);
                }

                model.Bookings = 0;
                model.GymName = await _gymRepository.GetGymNameByIdAsync(model.GymId);
                model.InstructorName = await _personalTrainerRepository.GetPtNameByIdAsync(model.InstructorId);
                model.EndDate = model.StartDate.AddMinutes(model.Duration);

                var workout = _converterHelper.ToWorkout(model, imageId, isNew: true);

                await _workoutRepository.CreateAsync(workout);

                return RedirectToAction(nameof(Index));
            }

            await LoadViewBags();

            return View(model);
        }

        // GET: Workouts/Edit/5
        [Authorize(Roles = "Admin, Employee, PersonalTrainer")]
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

            var model = _converterHelper.ToWorkoutViewModel(workout);
            await LoadViewBags();

            return View(model);
        }

        // POST: Workouts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Employee, PersonalTrainer")]
        public async Task<IActionResult> Edit(WorkoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.WorkoutImageFile != null && model.WorkoutImageFile.Length > 2 * 1024 * 1024) 
                {
                    ModelState.AddModelError("WorkoutImageFile", "The file size should not exceed 2 MB.");
                    return View(model);
                }

                try
                {
                    Guid imageId;
                    if (model.WorkoutImageFile != null)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.WorkoutImageFile, "workouts-pics");
                    }
                    else
                    {
                        imageId = model.WorkoutImageId;
                    }

                    var workout = _converterHelper.ToWorkout(model, imageId, false);

                    await _workoutRepository.UpdateAsync(workout);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _workoutRepository.ExistAsync(model.Id))
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

            await LoadViewBags();

            return View(model);
        }

        // GET: Workouts/Delete/5
        [Authorize(Roles = "Admin, Employee, PersonalTrainer")]
        public async Task<IActionResult> Delete(int? id)
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

            return View(workout);
        }

        // POST: Workouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Employee, PersonalTrainer")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workout = await _workoutRepository.GetByIdAsync(id);
            await _workoutRepository.DeleteAsync(workout);

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> MyBookings()
        {
            var userLoged = User.Identity.Name;
            if (userLoged != null)
            {
                string userId = await _userHelper.GetUserIdByEmailAsync(userLoged);

                var bookings = _bookingRepository.GetAll()
                    .Where(b => b.UserId == userId);
                var bookingViewModels = bookings.Select(b => _converterHelper.ToBookingViewModel(b)).ToList();

                return View(bookingViewModels);
            }

            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        public async Task<IActionResult> CreateBooking(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Workout not found.";
                return RedirectToAction("Details", new { id });
            }

            var userLoged = User.Identity.Name;
            if (userLoged != null)
            {
                string userId = await _userHelper.GetUserIdByEmailAsync(userLoged);
                var workout = await _workoutRepository.GetByIdAsync(id.Value);

                if (workout == null)
                {
                    TempData["ErrorMessage"] = "Workout not found.";
                    return RedirectToAction("Details", new { id });
                }

                var existingBooking = await _bookingRepository.GetBookingByUserAndWorkoutAsync(userId, workout.Id);
                if (existingBooking != null)
                {
                    TempData["ErrorMessage"] = "You have already booked this workout.";
                    return RedirectToAction("Details", new { id });
                }

                var maximumCapacityReached = await _bookingRepository.WorkoutMaximumCapacityReachedAsync(workout.Id);
                if (maximumCapacityReached)
                {
                    TempData["ErrorMessage"] = "Workout maximum capacity reached!";
                    return RedirectToAction("Details", new { id });
                }

                try
                {
                    var booking = new Booking
                    {
                        WorkoutId = id.Value,
                        WorkoutName = workout.Name,
                        UserId = userId,
                        TrainingDate = workout.StartDate,
                        GymId = workout.GymId,
                        GymName = workout.GymName,
                    };

                    await _bookingRepository.CreateBookingAsync(booking);
                    await _workoutRepository.IncrementBookingsAsync(workout.Id);

                    TempData["SuccessMessage"] = "Booking confirmed!";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An unexpected error occurred: " + ex.Message;
                }

                return RedirectToAction("Details", new { id });
            }

            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        public async Task<IActionResult> MyBookingsDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _bookingRepository.GetByIdAsync(id.Value);
            var workout = await _workoutRepository.GetByIdAsync(booking.WorkoutId);
            if (workout == null)
            {
                return NotFound();
            }

            ViewBag.Spots = workout.MaxCapacity - workout.Bookings;
            ViewBag.GymImage = await _gymRepository.GetGymImageAsync(workout.GymId);
            ViewBag.PtProfilePic = await _userHelper.GetUserPicAsync(workout.InstructorId);
            ViewBag.BookingStatus = booking.Status;
            ViewBag.ReservationDate = booking.ReservationDate;

            return View(workout);
        }

        [Authorize]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            await _bookingRepository.CancelBookingAsync(booking.Id);

            var workoutId = booking.WorkoutId;
            var workout = await _workoutRepository.GetByIdAsync(workoutId);

            workout.Bookings -= 1;
            await _workoutRepository.UpdateAsync(workout);
            return RedirectToAction("MyBookings");
        }

        public async Task LoadViewBags()
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
            ViewBag.GroupType = new SelectList(Enum.GetValues(typeof(Workout.GroupWorkoutType)).Cast<Workout.GroupWorkoutType>());
            ViewBag.IndividualType = new SelectList(Enum.GetValues(typeof(Workout.IndividualWorkoutType)).Cast<Workout.IndividualWorkoutType>());
            ViewBag.Difficulty = new SelectList(Enum.GetValues(typeof(Workout.WorkoutDifficulty)).Cast<Workout.WorkoutDifficulty>());
            ViewBag.Status = new SelectList(Enum.GetValues(typeof(Workout.WorkoutStatus)).Cast<Workout.WorkoutStatus>());
        }

        // Rate Workout
        public IActionResult RateWorkout(int workoutId)
        {
            var workout = _context.Workouts.Find(workoutId);
            if (workout == null)
            {
                return NotFound();
            }

            var userId = _userHelper.GetUserId(HttpContext.User);

            int userIdInt;
            if (!int.TryParse(userId, out userIdInt))
            {
                return Unauthorized();
            }

            var existingRating = _context.WorkoutRatings
                .FirstOrDefault(r => r.WorkoutId == workoutId && r.UserId == userIdInt);

            if (existingRating != null)
            {
                return RedirectToAction("Details", new { id = workoutId });
            }

            var ratingViewModel = new RatingViewModel
            {
                WorkoutId = workoutId
            };

            return View(ratingViewModel);
        }

        // Average Ratings
        public IActionResult GetAverageRating(int workoutId)
        {
            var ratings = _context.WorkoutRatings.Where(r => r.WorkoutId == workoutId).ToList();
            var averageRating = ratings.Any() ? ratings.Average(r => r.RatingValue) : 0;
            return Json(new { averageRating });
        }

    }
}