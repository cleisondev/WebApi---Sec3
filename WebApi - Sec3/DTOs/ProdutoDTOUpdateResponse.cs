using System.ComponentModel.DataAnnotations;
using WebApi___Sec3.Models;

namespace WebApi___Sec3.DTOs
{
    public class ProdutoDTOUpdateResponse
    {
        
        public int ProdutoId { get; set; }

        public string? Nome { get; set; }


        public string? Descricao { get; set; }


        public decimal Preco { get; set; }

        public string? ImagemUrl { get; set; }
        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }

        public int CategoriaId { get; set; }
    }
}
