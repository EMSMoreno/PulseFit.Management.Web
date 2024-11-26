namespace PulseFit.Management.Web.Data.Repositories
{
    // This interface will always receive a class or an entity. Since we don't know which entity, we use "<T>".

    // This interface is named IGenericRepository and will inject a "T", where "T" is a class.

    public interface IGenericRepository<T> where T : class
    {
        // GetAll() will be a method that returns all entities used by "T".
        IQueryable<T> GetAll();

        // GetByIdAsync(int id) will be a method that retrieves an entity by its ID asynchronously.
        Task<T> GetByIdAsync(int id);

        // CreateAsync(T entity) will be a method that creates a new entity asynchronously.
        Task CreateAsync(T entity);

        // UpdateAsync(T entity) will be a method that updates an existing entity asynchronously.
        Task UpdateAsync(T entity);

        // DeleteAsync(T entity) will be a method that deletes an existing entity asynchronously.
        Task DeleteAsync(T entity);

        // ExistAsync(int id) will be a method that checks if an entity exists by its ID asynchronously.
        Task<bool> ExistAsync(int id);
    }
}
