namespace PulseFit.Management.Web.Data.Repositories

{

    // Esta interface irá receber sempre uma classe ou uma entidade. Como não sabemos qual é a entidade, colocamos "<T>"

    // Esta interface vai-se chamar IGenericRepository e vai injetar um "T" em que o "T" é uma classe

    public interface IGenericRepository<T> where T : class

    {

        // GetAll() será um método que vai devolver todas as entidades que o "T" estiver a usar

        IQueryable<T> GetAll();

        // GetByIdAsync(int id) será um método que vai devolver uma entidade pelo seu ID de forma assíncrona

        Task<T> GetByIdAsync(int id);

        // CreateAsync(T entity) será um método que vai criar uma nova entidade de forma assíncrona

        Task CreateAsync(T entity);

        // UpdateAsync(T entity) será um método que vai atualizar uma entidade existente de forma assíncrona

        Task UpdateAsync(T entity);

        // DeleteAsync(T entity) será um método que vai apagar uma entidade existente de forma assíncrona

        Task DeleteAsync(T entity);

        // ExistAsync(int id) será um método que vai verificar se uma entidade existe pelo seu ID de forma assíncrona

        Task<bool> ExistAsync(int id);

    }

}


