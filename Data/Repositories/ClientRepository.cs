using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PulseFit.Management.Web.Data.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public ClientRepository(DataContext context, UserManager<User> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
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

        public async Task<IEnumerable<Client>> GetAllClientsWithRoleAsync(string roleName)
        {
            // Obter os IDs dos usuários que têm a função especificada
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            var userIds = usersInRole.Select(u => u.Id);

            // Retornar os clientes cujos UserId está na lista dos IDs com a função
            return await _context.Clients
                .Where(c => userIds.Contains(c.UserId))
                .ToListAsync();
        }

        public async Task<int?> GetClientIdByUserIdAsync(string userId)
        {
            return await _context.Clients
                .Where(c => c.UserId == userId)
                .Select(c => (int?)c.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<Client> GetByUserIdAsync(string userId)
        {
            return await _context.Clients
                .Include(c => c.User) // Inclui os dados relacionados ao User
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

    }
}
