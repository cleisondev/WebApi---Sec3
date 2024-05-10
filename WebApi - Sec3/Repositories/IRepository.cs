using System.Linq.Expressions;

namespace WebApi___Sec3.Repositories
{
    public interface IRepository<T>
    {

        Task<IEnumerable<T>> GetAllAsync();

        //T é o generico, get nome do metodo, predicate é um parametro do tipo Func de T, vai receber um T e retornar um boolean
        Task<T?> GetAsync(Expression<Func<T,bool>> predicate);
        T Create(T entity);
        T Update(T entity);
        T Delete(T entity);






    }
}
