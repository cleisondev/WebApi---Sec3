using WebApi___Sec3.Models;

namespace WebApi___Sec3.DTOs.Mappings
{
    public static class CategoriaDTOMappingExtensions
    {

        public static CategoriaDTO? ToCategoriaDTO(this Categoria categoria)
        {
            if(categoria is null)
                return null;

            return new CategoriaDTO
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemURl = categoria.ImagemURl
            };
        }

        public static Categoria? ToCategoria(this CategoriaDTO categoria) 
        {
            if (categoria is null)
                return null;

            return new Categoria
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemURl = categoria.ImagemURl
            };
        }

        public static IEnumerable<CategoriaDTO> ToCategoriaDTOList(this IEnumerable<Categoria> categoria)
        {
            if (categoria is null || !categoria.Any())
            {
                return new List<CategoriaDTO>();
            }

            //Aqui ele está projetando um novo objeto categoria em um objeto CategoriaDTO
            return categoria.Select(categoria => new CategoriaDTO
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemURl = categoria.ImagemURl
            }).ToList();

                
        }




    }
}
