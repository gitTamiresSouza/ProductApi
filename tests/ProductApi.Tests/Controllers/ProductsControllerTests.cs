using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductApi.Application.DTOs;
using ProductApi.Application.Services.Interfaces;
using ProductApi.Domain.Common;
using ProductApi.WebApi.Controllers;
using Xunit;

namespace ProductApi.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private ProductsController _controller;

        ProductDTO expectedProduct = new ProductDTO { Id = 1, Name= "teste1", Price = 2, Stock = 2};

        public ProductsControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _controller = new ProductsController(_mockProductService.Object);
        }

        [Fact]
        public async Task Get_ProductsWithFilter()
        {
            var filter = "{\"name\": \"notebook\"}";

            _mockProductService.Setup(x => x.ListAsync(It.IsAny<Filter>()))
                .ReturnsAsync(new List<ProductDTO> { expectedProduct });            

            var result = (await _controller.Get(filter)) as ObjectResult;

            Assert.Equivalent(new List<ProductDTO> { expectedProduct }, result.Value);
        }

        [Fact]
        public async Task Get_ProductsWithoutFilter()
        {
            _mockProductService.Setup(x => x.ListAsync(It.IsAny<Filter>()))
                .ReturnsAsync(new List<ProductDTO> { expectedProduct });

            var result = (await _controller.Get(string.Empty)) as ObjectResult;

            Assert.Equivalent(new List<ProductDTO> { expectedProduct }, result.Value);
        }

        [Fact]
        public async Task Get_Products_WithFilter_ReturnsNull()
        {
            var filter = "{\"name\": \"notebook\"}";

            _mockProductService.Setup(x => x.ListAsync(It.IsAny<Filter>()));

            var result = (await _controller.Get(filter)) as ObjectResult;

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Products not found", result.Value);
        }

        [Fact]
        public async Task Post_ValidProduct_ReturnsCreated()
        {
            _mockProductService.Setup(x => x.AddAsync(expectedProduct)).ReturnsAsync(true);

            var result = await _controller.Post(expectedProduct);

            var createdResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public async Task Post_NullProduct_ReturnsBadRequest()
        {
            var result = await _controller.Post(null);

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid Data", ((BadRequestObjectResult)result).Value);
        }

        [Fact]
        public async Task Post_ProductAlreadyExists_ReturnsNotFound()
        {
            _mockProductService.Setup(x => x.AddAsync(expectedProduct)).ReturnsAsync(false);

            var result = (await _controller.Post(expectedProduct)) as ObjectResult;

            Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Product already exists", result.Value);
        }

        [Fact]
        public async Task Put_ValidProduct_UpdatesAndReturnsOk()
        {
            _mockProductService.Setup(x => x.UpdateAsync(expectedProduct)).ReturnsAsync(true);

            var result = await _controller.Put(expectedProduct.Id, expectedProduct);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedProductDto = (ProductDTO)okResult.Value;
            Assert.Equal(expectedProduct, updatedProductDto);
        }

        [Fact]
        public async Task Put_ProductNotFound_ReturnsBadRequest()
        {
            _mockProductService.Setup(x => x.UpdateAsync(expectedProduct)).ReturnsAsync(false);

            var result = (await _controller.Put(expectedProduct.Id, expectedProduct)) as ObjectResult;

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Product not found", result.Value);
        }

        [Fact]
        public async Task Put_NullProduct_ReturnsBadRequest()
        {
            var result = await _controller.Put(It.IsAny<int>(), null);

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid Data", ((BadRequestObjectResult)result).Value);
        }

        [Fact]
        public async Task Put_MismatchedIds_ReturnsBadRequest()
        {
            var result = await _controller.Put(2, expectedProduct);

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid Id", ((BadRequestObjectResult)result).Value);
        }

        [Fact]
        public async Task Delete_ValidId_DeletesAndReturnsOk()
        {
            _mockProductService.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);

            var result = await _controller.Delete(It.IsAny<int>());

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Delete_ProductNotFound_ReturnsBadRequest()
        {
            _mockProductService.Setup(x => x.DeleteAsync(It.IsAny<int>())).ReturnsAsync(false);

            var result = await _controller.Delete(It.IsAny<int>());

            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Product not found", ((NotFoundObjectResult)result).Value);
        }
    }
}
