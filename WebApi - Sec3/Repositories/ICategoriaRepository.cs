using Microsoft.Identity.Client;
using WebApi___Sec3.Models;
using WebApi___Sec3.Pagination;
using X.PagedList;

namespace WebApi___Sec3.Repositories
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
       Task< IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriaFiltrosNome categoriaFiltroParams);
       Task< IPagedList<Categoria>> GetCategoriasAsync(CategoriaParameters categoriaParams);
    }
}
