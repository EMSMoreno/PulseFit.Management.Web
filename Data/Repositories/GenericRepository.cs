using Microsoft.EntityFrameworkCore;
using PulseFit.Management.Web.Data.Entities;

namespace PulseFit.Management.Web.Data.Repositories
{
    // A classe genérica GenericRepository vai implementar a interface genérica IGenericRepository
    // onde "T" é uma classe e também implementa a interface IEntity
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        private readonly DataContext _context;

        // Construtor da classe
        // "Ctrl + ." em cima do context e clicar em "Create and assign field context" vai criar este construtor automaticamente
        public GenericRepository(DataContext context)
        {
            _context = context;
        }

        // Método que devolve todas as entidades do tipo "T"
        public IQueryable<T> GetAll()
        {
            // Vai à tabela correspondente ao tipo "T" e traz todos os registos usando "AsNoTracking"
            // "AsNoTracking" indica que os objetos retornados não serão monitorados pelo contexto para alterações
            return _context.Set<T>().AsNoTracking();
        }

        // Método que devolve uma entidade pelo seu ID de forma assíncrona
        public async Task<T> GetByIdAsync(int id)
        {
            // Vai à tabela correspondente ao tipo "T" e procura o primeiro registo com o ID fornecido
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        // Método que cria uma nova entidade de forma assíncrona
        public async Task CreateAsync(T entity)
        {
            // Adiciona a nova entidade à tabela correspondente ao tipo "T"
            await _context.Set<T>().AddAsync(entity);
            // Guarda as alterações no contexto (base de dados)
            await SaveAllAsync();
        }

        // Método que atualiza uma entidade existente de forma assíncrona
        public async Task UpdateAsync(T entity)
        {
            // Atualiza a entidade na tabela correspondente ao tipo "T"
            _context.Set<T>().Update(entity);
            // Guarda as alterações no contexto (base de dados)
            await SaveAllAsync();
        }

        // Método que remove uma entidade existente de forma assíncrona
        public async Task DeleteAsync(T entity)
        {
            // Remove a entidade da tabela correspondente ao tipo "T"
            _context.Set<T>().Remove(entity);
            // Guarda as alterações no contexto (base de dados)
            await SaveAllAsync();
        }

        // Método que verifica se uma entidade com um determinado ID existe de forma assíncrona
        public async Task<bool> ExistAsync(int id)
        {
            // Verifica se existe alguma entidade na tabela correspondente ao tipo "T" com o ID fornecido
            return await _context.Set<T>().AnyAsync(e => e.Id == id);
        }

        // Método privado que guarda todas as alterações no contexto (base de dados)
        private async Task<bool> SaveAllAsync()
        {
            // SaveChangesAsync guarda as alterações e retorna o número de registos afetados
            // Se o número de registos afetados for maior que 0, retorna true
            return await _context.SaveChangesAsync() > 0;
        }
    }
}