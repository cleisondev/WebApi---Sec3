using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApi___Sec3.Context;

namespace WebApi___Sec3.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {

        protected readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync(); //Set do EF é usado para acessar uma colecao uma tabela
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public T Create(T entity)
        {
            _context.Set<T>().Add(entity);  
            //_context.SaveChanges();

            return entity;
        }
        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);
            //_context.SaveChanges();

            return entity;
        }
        public T Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            //_context.SaveChanges(); 
            return entity;
        }


      
    }
}
