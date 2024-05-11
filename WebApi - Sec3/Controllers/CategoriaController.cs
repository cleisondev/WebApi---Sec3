using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApi___Sec3.Context;
using WebApi___Sec3.DTOs;
using WebApi___Sec3.DTOs.Mappings;
using WebApi___Sec3.Filters;
using WebApi___Sec3.Models;
using WebApi___Sec3.Pagination;
using WebApi___Sec3.Repositories;
using WebApi___Sec3.Services;
using X.PagedList;

namespace WebApi___Sec3.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly IUnitOfWork _uof; 
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public CategoriaController(IConfiguration configuration, ILogger<CategoriaController> logger, IUnitOfWork uof, IMapper mapper)
        {
            _configuration = configuration;
            _logger = logger;
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet("produtos")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {

            try
            {
                
                var categoriasProdutos  = await _uof.CategoriaRepository.GetAllAsync();

                var categoriasDTO = new List<CategoriaDTO>();
                foreach (var categoria in categoriasDTO)
                {
                    var categoriaDTO = new CategoriaDTO
                    {
                        CategoriaId = categoria.CategoriaId,
                        Nome = categoria.Nome,
                        ImagemURl = categoria.ImagemURl
                    };

                    categoriasDTO.Add(categoriaDTO);
                }


                var categoriasDto = categoriasProdutos.ToCategoriaDTOList();

                return Ok(categoriasDto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema a tratar a sua solicitação");
            }
        }


        [HttpGet("{id:int}", Name = "ObterCategoria")] 
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {

            var categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);


            if (categoria is null)
                return NotFound("Categoria não encontrada");


            var categoriaDto = categoria.ToCategoriaDTO();


            return Ok(categoriaDto);
        }

        [HttpGet("pagination")]
        public async  Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriaParameters categoriasParameters)
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasAsync(categoriasParameters);

            //Criando uma variavel anonima
            return obterCategorias(categorias);

        }


        [HttpGet("filter/nome/pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasFiltradas([FromQuery] CategoriaFiltrosNome categoriaFiltros)
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasFiltroNomeAsync(categoriaFiltros);


            return obterCategorias(categorias);
        }



        private ActionResult<IEnumerable<CategoriaDTO>> obterCategorias(IPagedList<Categoria> categorias)
        {
            var metadata = new
            {
                categorias.Count,
                categorias.PageSize,
                categorias.PageCount,
                categorias.TotalItemCount,
                categorias.HasNextPage,
                categorias.HasPreviousPage
            };

            //passando no response
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriasDTO = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

            return Ok(categoriasDTO);
        }

       




        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDTO)
        {

            if (categoriaDTO is null)
                return BadRequest("Categoria nula");

      
            var categoria = categoriaDTO.ToCategoria();

            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            await _uof.CommitAsync();

            var novaCategoriaDTO = categoriaCriada.ToCategoriaDTO();

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = novaCategoriaDTO.CategoriaId }, novaCategoriaDTO);

        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDTO)
        {

            if (id != categoriaDTO.CategoriaId)
                return BadRequest();

            var categoria = categoriaDTO.ToCategoria();

            _uof.CategoriaRepository.Update(categoria);
            await _uof.CommitAsync();


            var categoriaAtualizadaDTO = categoria.ToCategoriaDTO();

            return Ok(categoriaAtualizadaDTO);

        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);

            if (categoria is null)
                return NotFound();

           _uof.CategoriaRepository.Delete(categoria);
           await _uof.CommitAsync();

            var categoriaExcluidaDTO = categoria.ToCategoriaDTO();

            return Ok(categoriaExcluidaDTO);

        }

    }
}
