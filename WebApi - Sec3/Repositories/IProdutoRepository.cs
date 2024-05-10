using WebApi___Sec3.Models;
using WebApi___Sec3.Pagination;
using X.PagedList;

namespace WebApi___Sec3.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams) ;

        Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams);
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);



    }
}
