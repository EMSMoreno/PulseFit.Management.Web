using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data;
using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Repositories
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly DataContext _context;

        public MembershipRepository(DataContext context)
        {
            _context = context;
        }

        // Returns a list of subscriptions with outstanding fees
        public async Task<IEnumerable<Membership>> GetPendingFeesAsync()
        {
            return await _context.Memberships
                .Where(m => m.IsPendingFee)
                .Include(m => m.User) // Includes related user information
                .ToListAsync();
        }

        // Returns signup history for a specific user
        public async Task<IEnumerable<Membership>> GetMembershipHistoryAsync(int userId)
        {
            return await _context.Memberships
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.StartDate) // Sorts by start date, from newest to oldest
                .ToListAsync();
        }

        // Additional method: Add new registration for a user
        public async Task AddMembershipAsync(Membership membership)
        {
            _context.Memberships.Add(membership);
            await _context.SaveChangesAsync();
        }
    }
}
