using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi___Sec3.Controllers;

namespace ApiCatalogoXUnitTests.UnitTests
{
    public class DeleteProdutoUnitTest : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;

        public DeleteProdutoUnitTest(ProdutosUnitTestController controller)//Injetando as propriedades do controller 
        {
            _controller = new ProdutosController(controller.repository, controller.mapper);
        }


        [Fact]
        public async Task deleteReturnResultOk()
        {
            var prodid = 14;

            var result = await _controller.Delete(prodid);

            result.Should().NotBeNull();
            result.Result.Should().BeOfType<OkObjectResult>();

        }


        [Fact]
        public async Task deleteReturnNotFound()
        {
            var prodid = 1;

            var result = await _controller.Delete(prodid);

            result.Should().NotBeNull();
            result.Result.Should().BeOfType<NotFoundObjectResult>();

        }

    }
}
