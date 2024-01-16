using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Entities;
using Xunit;
using Moq;
using ProductApi.Domain.Common;

namespace ProductApi.Tests.Repository
{
    public class ProductRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly ProductRepository _repository;
        public ProductRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
            _context = new AppDbContext(options);
            _context.Database.EnsureCreated();
            _repository = new ProductRepository(_context);
        }

        [Fact]
        public async Task ListAsync_ReturnsAllProducts_WithoutFilter()
        {
            var product = new ProductEntity
            {
                Id = 1,
                Name = "teste 1",
                Price = 2,
                Stock = 2
            };
            await _context.Products.AddRangeAsync(new List<ProductEntity> { product });
            await _context.SaveChangesAsync();

            var result = await _repository.ListAsync(null);

            Assert.Equivalent(new List<ProductEntity> { product }, result);
        }

        [Fact]
        public async Task ListAsync_ReturnsAllProducts_WithFilter()
        {
            var product = new ProductEntity
            {
                Id = 2,
                Name = "teste 1",
                Price = 2,
                Stock = 2
            };
            await _context.Products.AddRangeAsync(new List<ProductEntity> { product });
            await _context.SaveChangesAsync();

            var result = await _repository.ListAsync(It.IsAny<Filter>());

            Assert.Equivalent(new List<ProductEntity> { product }, result);
        }

        [Fact]
        public async Task AddAsync_NewProduct_ReturnsTrue()
        {
            ProductEntity expectedProductEntity = new ProductEntity { Id = 3, Name = "teste 1", Price = 2, Stock = 2 };

            var result = await _repository.AddAsync(expectedProductEntity);

            Assert.True(result);
        }

        [Fact]
        public async Task AddAsync_ExistingProduct_ReturnsFalse()
        {
            var product = new ProductEntity
            {
                Id = 4,
                Name = "teste 1",
                Price = 2,
                Stock = 2
            };
            await _context.Products.AddRangeAsync(new List<ProductEntity> { product });
            await _context.SaveChangesAsync();
            
            var result = await _repository.AddAsync(product);

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateAsync_ExistingProduct_ReturnsTrue()
        {
            var product = new ProductEntity { Id = 5, Name = "teste 1", Price = 2, Stock = 2 };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var result = await _repository.UpdateAsync(product);

            Assert.True(result);
        }

        [Fact]
        public async Task UpdateAsync_ProductNotFound_ReturnsFalse()
        {
            ProductEntity expectedProductEntity = new ProductEntity { Id = 6, Name = "teste 1", Price = 2, Stock = 2 };

            var result = await _repository.UpdateAsync(expectedProductEntity);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_ExistingProduct_ReturnsTrue()
        {
            await _context.Products.AddAsync(new ProductEntity { Id = 6, Name = "teste 1", Price = 2, Stock = 2 });
            await _context.SaveChangesAsync();

            var result = await _repository.DeleteAsync(6);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ProductNotFound_ReturnsFalse()
        {
            var result = await _repository.DeleteAsync(7);

            Assert.False(result);
        }
    }
}
