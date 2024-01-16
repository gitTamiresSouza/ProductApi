using AutoMapper;
using ProductApi.Application.DTOs;
using ProductApi.Domain.Entities;

namespace ProductApi.Application.Mappings
{
    public class DTOToEntityMapping : Profile
    {
        public DTOToEntityMapping() 
        {
            CreateMap<ProductDTO, ProductEntity>();
        }        
    }
}
