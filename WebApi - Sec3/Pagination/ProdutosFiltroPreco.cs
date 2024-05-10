namespace WebApi___Sec3.Pagination
{
    public class ProdutosFiltroPreco : QueryStringParameters
    {
        public decimal? preco { get; set; }
        public string? PrecoCriterio { get; set; }
    }
}
