using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;

namespace Ecommerce.API;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<Product, ProductDto>().ReverseMap();

        CreateMap<AddProductDto, Product>()
                 .ForMember(dest => dest.ImageData, opt => opt.Ignore()) // If Product has Image
                 .ForSourceMember(src => src.ImageData, opt => opt.DoNotValidate()); // Ignore ImageData from source

    }

}
