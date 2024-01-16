using ProductApi.Application.DTOs;
using ProductApi.Domain.Common;

namespace ProductApi.Application.Services.Interfaces
{
    public interface IProductService
    {
        public Task<List<ProductDTO>> ListAsync(Filter filter);

        public Task<bool> AddAsync(ProductDTO product);

        public Task<bool> UpdateAsync(ProductDTO product);

        public Task<bool> DeleteAsync(int productId);
    }
}
