using WebApi___Sec3.Context;

namespace WebApi___Sec3.Repositories
{
    public interface IUnitOfWork
    {
        IProdutoRepository ProdutoRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }

        Task CommitAsync();



    }
}
