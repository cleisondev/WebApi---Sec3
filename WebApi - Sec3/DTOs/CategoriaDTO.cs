using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using WebApi___Sec3.Models;

namespace WebApi___Sec3.DTOs
{
    public class CategoriaDTO
    {
        public int CategoriaId { get; set; }

        [Required]
        [StringLength(80)]
        public string? Nome { get; set; }

        [Required]
        [StringLength(300)]
        public string? ImagemURl { get; set; }



    }
}
