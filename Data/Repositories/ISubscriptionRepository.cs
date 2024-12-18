﻿using PulseFit.Management.Web.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PulseFit.Management.Web.Data.Repositories
{
    public interface ISubscriptionRepository : IGenericRepository<Subscription>
    {
        Task<IEnumerable<Subscription>> GetAllActiveSubscriptionsAsync();
        Task<Subscription> GetByIdWithDetailsAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
        Task<IEnumerable<Subscription>> GetSubscriptionsByGymAsync(int gymId);

        Task<bool> ExistsExclusiveSubscriptionAsync(SubscriptionType subscriptionType);

        Task<IEnumerable<Subscription>> GetSubscriptionsByGymLocationAsync(string location);

    }
}
