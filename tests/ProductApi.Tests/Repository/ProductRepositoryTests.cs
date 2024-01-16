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
        ProductEntity firstProduct = new ProductEntity { Id = 1, Name = "teste 1", Price = 2, Stock = 2 };
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
            await _context.Products.AddRangeAsync(new List<ProductEntity> { firstProduct });
            await _context.SaveChangesAsync();

            var result = await _repository.ListAsync(null);

            Assert.Equivalent(new List<ProductEntity> { firstProduct }, result);
        }

        [Fact]
        public async Task ListAsync_ReturnsAllProducts_WithFilter()
        {
            var productExists = await _context.Products.FirstOrDefaultAsync(x => x.Id == 1);
            if (productExists == null)
            {
                await _context.Products.AddAsync(firstProduct);
                await _context.SaveChangesAsync();
            }

            var result = await _repository.ListAsync(It.IsAny<Filter>());

            Assert.Equivalent(new List<ProductEntity> { firstProduct }, result);
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
            var productExists = await _context.Products.FirstOrDefaultAsync(x => x.Id == 1);
            if (productExists == null)
            {
                await _context.Products.AddAsync(firstProduct);
                await _context.SaveChangesAsync();
            }
            
            var result = await _repository.AddAsync(firstProduct);

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateAsync_ExistingProduct_ReturnsTrue()
        {
            var result = await _repository.UpdateAsync(firstProduct);

            Assert.True(result);
        }

        [Fact]
        public async Task UpdateAsync_ProductNotFound_ReturnsFalse()
        {
            ProductEntity expectedProductEntity = new ProductEntity { Id = 5, Name = "teste 1", Price = 2, Stock = 2 };

            var result = await _repository.UpdateAsync(expectedProductEntity);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_ExistingProduct_ReturnsTrue()
        {
            var result = await _repository.DeleteAsync(firstProduct.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ProductNotFound_ReturnsFalse()
        {
            var result = await _repository.DeleteAsync(5);

            Assert.False(result);
        }
    }
}
