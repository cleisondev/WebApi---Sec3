using Microsoft.EntityFrameworkCore;
using WebApi___Sec3.Context;
using WebApi___Sec3.Models;
using WebApi___Sec3.Pagination;
using X.PagedList;

namespace WebApi___Sec3.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context) : base(context) { }

 
        public async Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams)
        {
            var produtos = await GetAllAsync();
            var produtosordenados = produtos.OrderBy(p => p.ProdutoId).AsQueryable();

            //var resultado = PagedList<Produto>.ToPagedList(produtosordenados, produtosParams.PageNumber, produtosParams.PageSize);
            var resultado = await produtosordenados.ToPagedListAsync(produtosParams.PageNumber, produtosParams.PageSize);
            return resultado;
        }

        public async  Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams)
        {
            var produtos = await GetAllAsync();

            if(produtosFiltroParams.preco.HasValue && !string.IsNullOrEmpty(produtosFiltroParams.PrecoCriterio))
            {
                if(produtosFiltroParams.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco > produtosFiltroParams.preco.Value).OrderBy(p => p.Preco);
                }
                else if(produtosFiltroParams.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco < produtosFiltroParams.preco.Value).OrderBy(p => p.Preco);
                }
                else if (produtosFiltroParams.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco == produtosFiltroParams.preco.Value).OrderBy(p => p.Preco);
                }
            }

            //var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos.AsQueryable(), produtosFiltroParams.PageNumber, produtosFiltroParams.PageSize);
            var produtosFiltrados = await produtos.ToPagedListAsync(produtosFiltroParams.PageNumber, produtosFiltroParams.PageSize);


            return produtosFiltrados;

        }

        public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id)
        {
            var produtos = await GetAllAsync();
            var produtosCategoria = produtos.Where(p => p.CategoriaId == id);

            return produtosCategoria;
        }
    }
}
