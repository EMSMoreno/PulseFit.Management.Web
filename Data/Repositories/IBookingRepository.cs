using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task <IEnumerable<Booking>> GetBookingsByUserAsync(int  userId);

        Task<int> GetAvailableSlotsAsync(int workoutId);

        Task CreateBookingAsync(Booking booking);

        Task CancelBookingAsync(int bookingId);
    }
}
