using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi___Sec3.Controllers;
using WebApi___Sec3.DTOs;
using WebApi___Sec3.Models;

namespace ApiCatalogoXUnitTests.UnitTests
{
    public class PostProdutoUnitTest : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;

        public PostProdutoUnitTest(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller.repository, controller.mapper);
        }

        [Fact]
        private async Task PostProdutoReturnCreatedStatusCode()
        {
            //Arrange
            var novoProdutoDTO = new ProdutoDTO
            {
                Nome = "Cleison Nunes",
                Descricao = "cleison",
                Preco = 19,
                ImagemUrl = "cleison.jpssssss",
                CategoriaId = 1
            };

            //Act
            var data = await _controller.Post(novoProdutoDTO);

            //Assert

            var createdResult = data.Result.Should().BeOfType<CreatedAtRouteResult>();
            createdResult.Subject.StatusCode.Should().Be(201);

        }



        [Fact]
        private async Task PostProdutoReturnBadRequest()
        {
            //Arrange
            ProdutoDTO novoProdutoDTO = null;

            //Act
            var data = await _controller.Post(novoProdutoDTO);

            //Assert

            var badRequestResult = data.Result.Should().BeOfType<BadRequestResult>();
            badRequestResult.Subject.StatusCode.Should().Be(400);

        }




    }
}
