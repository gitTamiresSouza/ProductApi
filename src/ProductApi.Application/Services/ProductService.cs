using AutoMapper;
using ProductApi.Application.DTOs;
using ProductApi.Application.Services.Interfaces;
using ProductApi.Domain.Common;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Interfaces;

namespace ProductApi.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, IMapper mapper) 
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ProductDTO>> ListAsync(Filter filter) => _mapper.Map<List<ProductDTO>>(await _repository.ListAsync(filter));

        public async Task<bool> AddAsync(ProductDTO product) => await _repository.AddAsync(_mapper.Map<ProductEntity>(product));

        public async Task<bool> UpdateAsync(ProductDTO product) => await _repository.UpdateAsync(_mapper.Map<ProductEntity>(product));

        public async Task<bool> DeleteAsync(int productId) => await _repository.DeleteAsync(productId);
    }
}
