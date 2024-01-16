using AutoMapper;
using Moq;
using ProductApi.Application.DTOs;
using ProductApi.Application.Services;
using ProductApi.Domain.Common;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Interfaces;
using Xunit;

namespace ProductApi.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private ProductService _service;
        ProductEntity expectedProductEntity = new ProductEntity { Id = 1, Name = "teste1", Price = 2, Stock = 2 };
        ProductDTO expectedProductDTO = new ProductDTO { Id = 1, Name = "teste1", Price = 2, Stock = 2 };
        public ProductServiceTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new ProductService(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task List_ReturnsListFromRepository_WithoutFilter()
        {
            _mockRepository.Setup(x => x.ListAsync(It.IsAny<Filter>()))
                .ReturnsAsync(new List<ProductEntity> { expectedProductEntity });

            _mockMapper.Setup(x => x.Map<List<ProductDTO>>(new List<ProductEntity> { expectedProductEntity }))
                .Returns(new List<ProductDTO>() { expectedProductDTO });

            var result = await _service.ListAsync(null);

            Assert.Equal(new List<ProductDTO>() { expectedProductDTO }, result);
        }

        [Fact]
        public async Task List_ReturnsListFromRepository_WithFilter()
        {
            _mockRepository.Setup(x => x.ListAsync(It.IsAny<Filter>()))
                .ReturnsAsync(new List<ProductEntity> { expectedProductEntity });

            _mockMapper.Setup(x => x.Map<List<ProductDTO>>(new List<ProductEntity> { expectedProductEntity }))
                .Returns(new List<ProductDTO>() { expectedProductDTO });

            var result = await _service.ListAsync(It.IsAny<Filter>());

            Assert.Equal(new List<ProductDTO>() { expectedProductDTO }, result);
        }

        [Fact]
        public async Task AddAsync_MapsProduct_ReturnsTrue()
        {
            _mockMapper.Setup(x => x.Map<ProductEntity>(expectedProductDTO))
                .Returns(expectedProductEntity);
            _mockRepository.Setup(x => x.AddAsync(expectedProductEntity))
                .ReturnsAsync(true);

            var result = await _service.AddAsync(expectedProductDTO);

            Assert.True(result);
        }

        [Fact]
        public async Task AddAsync_MapsProduct_ReturnsFalse()
        {
            _mockRepository.Setup(x => x.AddAsync(expectedProductEntity))
                .ReturnsAsync(false);

            var result = await _service.AddAsync(expectedProductDTO);

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateAsync_MapsProduct_ReturnsTrue()
        {
            _mockMapper.Setup(x => x.Map<ProductEntity>(expectedProductDTO))
                .Returns(expectedProductEntity);
            _mockRepository.Setup(x => x.UpdateAsync(expectedProductEntity))
                .ReturnsAsync(true);

            var result = await _service.UpdateAsync(expectedProductDTO);

            Assert.True(result);
        }

        [Fact]
        public async Task UpdateAsync_MapsProduct_ReturnsFalse()
        {
            _mockRepository.Setup(x => x.UpdateAsync(expectedProductEntity))
                .ReturnsAsync(false);

            var result = await _service.UpdateAsync(expectedProductDTO);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_MapsProduct_ReturnsTrue()
        {
            _mockMapper.Setup(x => x.Map<ProductEntity>(expectedProductDTO))
                .Returns(expectedProductEntity);
            _mockRepository.Setup(x => x.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            var result = await _service.DeleteAsync(It.IsAny<int>());

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_MapsProduct_ReturnsFalse()
        {
            _mockRepository.Setup(x => x.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            var result = await _service.DeleteAsync(It.IsAny<int>());

            Assert.False(result);
        }

    }
}
