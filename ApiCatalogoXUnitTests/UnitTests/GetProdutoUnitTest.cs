using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public class GetProdutoUnitTest : IClassFixture<ProdutosUnitTestController>   
    {
        private readonly ProdutosController _controller;

        public GetProdutoUnitTest(ProdutosUnitTestController controller)//Injetando as propriedades do controller 
        {
            _controller = new ProdutosController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task GetProdutoById_OkResult()//Criando um teste de 200 ok
        {
            //Arrange
            var prodId = 14;

            //Act
            var data = await _controller.GetId(prodId);

            //Assert - Usando Xunit
            var okResult = Assert.IsType<OkObjectResult>(data.Result);
            Assert.Equal(200, okResult.StatusCode);

            //Asser (fluentassertions)
            data.Result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);    
        }

        [Fact]
        public async Task GetProdutoById_NotFound()//Criando um teste de 200 ok
        {
            //Arrange
            var prodId = 9999;

            //Act
            var data = await _controller.GetId(prodId);

            ////Assert - Usando Xunit
            //var okResult = Assert.IsType<OkObjectResult>(data.Result);
            //Assert.Equal(200, okResult.StatusCode);

            //Asser (fluentassertions)
            data.Result.Should().BeOfType<NotFoundObjectResult>()
                .Which.StatusCode.Should().Be(404);
        }


        [Fact]
        public async Task GetProdutoById_BadRequest()//Criando um teste de 200 ok
        {
            //Arrange
            var prodId = -1;

            //Act
            var data = await _controller.GetId(prodId);

            ////Assert - Usando Xunit
            //var okResult = Assert.IsType<OkObjectResult>(data.Result);
            //Assert.Equal(200, okResult.StatusCode);

            //Asser (fluentassertions)
            data.Result.Should().BeOfType<BadRequestObjectResult>()
                .Which.StatusCode.Should().Be(400);
        }


        [Fact]
        public async Task GetProduto_ListOfProdutoDTO()//Criando um teste de 200 ok
        {
            //Arrange
            var prodId = -1;

            //Act
            var data = await _controller.Get();

            ////Assert - Usando Xunit
            //var okResult = Assert.IsType<OkObjectResult>(data.Result);
            //Assert.Equal(200, okResult.StatusCode);

            //Asser (fluentassertions)
            data.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<IEnumerable<ProdutoDTO>>()
                .And.NotBeNull();
                
        }


        [Fact]
        public async Task GetProduto_Return_BadRequestResult()//Criando um teste de 200 ok
        {
            //Arrange
            var prodId = -1;

            //Act
            var data = await _controller.Get();

            ////Assert - Usando Xunit
            //var okResult = Assert.IsType<OkObjectResult>(data.Result);
            //Assert.Equal(200, okResult.StatusCode);

            //Asser (fluentassertions)
            data.Result.Should().BeOfType<BadRequestResult>();

        }



    }
}
