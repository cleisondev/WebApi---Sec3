using Microsoft.EntityFrameworkCore;
using WebApi___Sec3.Context;
using WebApi___Sec3.Models;
using WebApi___Sec3.Pagination;
using X.PagedList;

namespace WebApi___Sec3.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context) : base(context) { }

        public async Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriaFiltrosNome categoriaFiltrosParams)
        {
            var categorias = await GetAllAsync();
            

            if (!string.IsNullOrEmpty(categoriaFiltrosParams.Nome))
            {

                categorias = categorias.Where(p => p.Nome.Contains(categoriaFiltrosParams.Nome));
            }


            //var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias.AsQueryable(), categoriaFiltrosParams.PageNumber, categoriaFiltrosParams.PageSize);
            var categoriasFiltradas = await categorias.ToPagedListAsync(categoriaFiltrosParams.PageNumber, categoriaFiltrosParams.PageSize);


            return categoriasFiltradas;

        }
        public async Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriaParameters categoriaParams)
        {
            var categorias = await GetAllAsync();
            var categoriasOrdenadasAsync = categorias.OrderBy(p => p.CategoriaId).AsQueryable();

            //var resultado = IPagedList<Categoria>.ToPagedList(categoriasOrdenadasAsync, categoriaParams.PageNumber, categoriaParams.PageSize);
            var resultado = await categoriasOrdenadasAsync.ToPagedListAsync(categoriaParams.PageNumber, categoriaParams.PageSize);
            return resultado;
        }

    }

}
