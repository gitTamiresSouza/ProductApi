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
            ProductEntity expectedProductEntity = new ProductEntity { Id = 1, Name = "teste 1", Price = 2, Stock = 2 };
            await _context.Products.AddRangeAsync(new List<ProductEntity> { expectedProductEntity });
            await _context.SaveChangesAsync();

            var result = await _repository.ListAsync(null);

            Assert.Equivalent(new List<ProductEntity> { expectedProductEntity }, result);
        }

        [Fact]
        public async Task ListAsync_ReturnsAllProducts_WithFilter()
        {
            ProductEntity expectedProductEntity = new ProductEntity { Id = 2, Name = "teste 1", Price = 2, Stock = 2 };
            await _context.Products.AddRangeAsync(new List<ProductEntity> { expectedProductEntity });
            await _context.SaveChangesAsync();

            var result = await _repository.ListAsync(It.IsAny<Filter>());

            Assert.Equivalent(new List<ProductEntity> { expectedProductEntity }, result);
        }
    }
}
