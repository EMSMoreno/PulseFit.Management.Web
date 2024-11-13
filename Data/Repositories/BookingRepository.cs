using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly DataContext _context;

        public BookingRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task CancelBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null || booking.Status != Booking.BookingStatus.Reserved)
            {
                throw new Exception("Booking not Found or already canceled/completed.");
            }

            // Penalidade para cancelamentos em menos de 24 horas
            if ((booking.TrainingDate - DateTime.Now).TotalHours < 24)
            {
                //TODO : Lógica para aplicação de penalidade, se aplicável
            }

            booking.Status = Booking.BookingStatus.Canceled;

            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task CreateBookingAsync(Booking booking)
        {
            booking.ReservationDate = DateTime.Now;
            booking.Status = Booking.BookingStatus.Reserved;
            
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetAvailableSlotsAsync(int workoutId)
        {
            var workout = await _context.Workouts.FindAsync(workoutId);
            int reservedSlots = workout.Bookings;
            return workout.MaxCapacity - reservedSlots;
        }

        public async Task<Booking> GetBookingByUserAndWorkoutAsync(string userId, int workoutId)
        {
            return await _context.Bookings
                .FirstOrDefaultAsync(b => b.UserId == userId && b.WorkoutId == workoutId);
        }


        public async Task<IEnumerable<Booking>> GetBookingsByUserAsync(string userId)
        {
            return await _context.Bookings
                .Where(b => b.UserId == userId)
                .OrderBy(b => b.TrainingDate)
                .ToListAsync();
        }

        public async Task<bool> WorkoutMaximumCapacityReachedAsync(int workoutId)
        {
            var workout = await _context.Workouts
                .FirstOrDefaultAsync(w => w.Id == workoutId);
            
            if (workout.MaxCapacity <= workout.Bookings)
            {
                return true;
            }

            return false;
        }
    }
}
