﻿using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data;
using PulseFit.Management.Web.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PulseFit.Management.Web.Data.Repositories
{
    public class UserSubscriptionRepository : GenericRepository<UserSubscription>, IUserSubscriptionRepository
    {
        private readonly DataContext _context;

        public UserSubscriptionRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserSubscription>> GetAllAsync()
        {
            return await _context.UserSubscriptions
                .Include(us => us.Subscription)
                .Include(us => us.Client)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserSubscription>> GetUserSubscriptionsAsync(int clientId)
        {
            return await _context.UserSubscriptions
                .Include(us => us.Subscription)
                .Where(us => us.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<UserSubscription> GetUserSubscriptionByIdAsync(int id)
        {
            return await _context.UserSubscriptions
                .Include(us => us.Subscription)
                .FirstOrDefaultAsync(us => us.Id == id);
        }

        public async Task<UserSubscription> GetActiveUserSubscriptionAsync(string userId, int subscriptionId)
        {
            return await _context.UserSubscriptions
                .Include(us => us.Subscription)
                .FirstOrDefaultAsync(us => us.UserId == userId && us.SubscriptionId == subscriptionId && us.Status == SubscriptionStatus.Active);
        }

    }
}
