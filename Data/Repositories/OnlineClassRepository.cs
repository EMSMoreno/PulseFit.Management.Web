using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data;
using PulseFit.Management.Web.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PulseFit.Management.Web.Data.Repositories
{
    public class OnlineClassRepository : GenericRepository<OnlineClass>, IOnlineClassRepository
    {
        private readonly DataContext _context;

        public OnlineClassRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OnlineClass>> GetAllAsync()
        {
            return await _context.OnlineClasses.ToListAsync();
        }
    }
}
