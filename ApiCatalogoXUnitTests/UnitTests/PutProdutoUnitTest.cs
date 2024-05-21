using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi___Sec3.Controllers;
using WebApi___Sec3.DTOs;

namespace ApiCatalogoXUnitTests.UnitTests
{
    public class PutProdutoUnitTest : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;

        public PutProdutoUnitTest(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller.repository, controller.mapper);
        }


        [Fact]
        public async Task PutProdutoReturnOkResult()
        {
            var prodId = 16;

            var updateProduto = new ProdutoDTO
            {
                ProdutoId = prodId,
                Nome = "Cleison",
                Descricao = "cleison",
                Preco = 10,
                ImagemUrl = "cleison.jesusss",
                CategoriaId = 1
            };

            var result = await _controller.Put(prodId,updateProduto);

            result.Should().NotBeNull();
            result.Result.Should().BeOfType<OkObjectResult>(); 

        }


        [Fact]
        public async Task PutProdutoReturnBadRequestResult()
        {
            var prodId = 1;

            var updateProduto = new ProdutoDTO
            {
                ProdutoId = 2,
                Nome = "Cleison",
                Descricao = "cleison",
                Preco = 10,
                ImagemUrl = "cleison.jesusss",
                CategoriaId = 1
            };

            var result = await _controller.Put(prodId, updateProduto);

            result.Result.Should().BeOfType<BadRequestResult>().Which.StatusCode.Should().Be(400);

        }






    }
}
