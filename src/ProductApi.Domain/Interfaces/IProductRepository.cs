using ProductApi.Domain.Common;
using ProductApi.Domain.Entities;

namespace ProductApi.Domain.Interfaces
{
    public interface IProductRepository
    {
        public Task<List<ProductEntity>> ListAsync(Filter filter);

        public Task<bool> AddAsync(ProductEntity product);

        public Task<bool> UpdateAsync(ProductEntity product);

        public Task<bool> DeleteAsync(int productId);
    }
}
