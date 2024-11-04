using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PulseFit.Management.Web.Data.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        private readonly DataContext _context;

        public ClientRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Client>> GetAllWithUsersAsync()
        {
            return await _context.Clients
                .Include(c => c.User) // Inclui os dados relacionados ao User
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Client> GetByIdWithUserAsync(int id)
        {
            return await _context.Clients
                .Include(c => c.User) // Inclui o User para evitar NullReferenceException
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
