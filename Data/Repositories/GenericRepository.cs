using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    // The GenericRepository generic class will implement the IGenericRepository generic interface
    // where "T" is a class and also implements the IEntity interface
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        private readonly DataContext _context;

        // Class constructor
        // "Ctrl + ." above the context and clicking on "Create and assign field context" will create this constructor automatically
        public GenericRepository(DataContext context)
        {
            _context = context;
        }

        // Method that returns all entities of type "T"
        public IQueryable<T> GetAll()
        {
            // Go to the table corresponding to type "T" and bring up all the records using "AsNoTracking"
            // "AsNoTracking" indicates that the returned objects will not be tracked by the context for changes
            return _context.Set<T>().AsNoTracking();
        }

        // Method that returns an entity by its ID asynchronously
        public async Task<T> GetByIdAsync(int id)
        {
            // Go to the table corresponding to type "T" and look for the first record with the given ID
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        // Method that creates a new entity asynchronously
        public async Task CreateAsync(T entity)
        {
            // Adds the new entity to the table corresponding to type "T"
            await _context.Set<T>().AddAsync(entity);
            // Saves changes to context (database)
            await SaveAllAsync();
        }

        // Method that updates an existing entity asynchronously
        public async Task UpdateAsync(T entity)
        {
            // Updates the entity in the table corresponding to type "T"
            _context.Set<T>().Update(entity);
            // Saves changes to context (database)
            await SaveAllAsync();
        }

        // Method that removes an existing entity asynchronously
        public async Task DeleteAsync(T entity)
        {
            // Removes the entity from the table corresponding to type "T"
            _context.Set<T>().Remove(entity);
            // Saves changes to context (database)
            await SaveAllAsync();
        }

        // Method that checks whether an entity with a given ID exists asynchronously
        public async Task<bool> ExistAsync(int id)
        {
            // Checks if there is any entity in the table corresponding to type "T" with the given ID
            return await _context.Set<T>().AnyAsync(e => e.Id == id);
        }

        // Private method that saves all changes to the context (database)
        private async Task<bool> SaveAllAsync()
        {
            // SaveChangesAsync saves changes and returns the number of records affected
            // If the number of affected records is greater than 0, return true
            return await _context.SaveChangesAsync() > 0;
        }
    }
}