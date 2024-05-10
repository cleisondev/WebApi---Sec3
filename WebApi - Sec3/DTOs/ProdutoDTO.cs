using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebApi___Sec3.Models;

namespace WebApi___Sec3.DTOs
{
    public class ProdutoDTO
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(20, ErrorMessage = "O nome deve ter ebtre 5 e 20 caracteres", MinimumLength = 5)]
        //[PrimeiraLetraMaiuscula]
        public string? Nome { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 10)]
        public string? ImagemUrl { get; set; }

        public int CategoriaId { get; set; }

        [JsonIgnore]
        public Categoria? Categoria { get; set; }


    }
}
