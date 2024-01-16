using AutoMapper;
using ProductApi.Application.DTOs;
using ProductApi.Domain.Entities;

namespace ProductApi.Application.Mappings
{
    public class EntityToDTO : Profile
    {
        public EntityToDTO() 
        {
            CreateMap<ProductEntity, ProductDTO>();
        }
    }
}
