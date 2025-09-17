using AutoMapper;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.IServices;
using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GenericResponseDto<IEnumerable<ProductDto>>> GetAllProductsAsync()
    {
        var products = await _unitOfWork.ProductRepo
             .FindAllAsync(null, null, null);

        var productDto = _mapper.Map<IEnumerable<ProductDto>>(products);



        return new GenericResponseDto<IEnumerable<ProductDto>>()
        {
            Status = 200,
            Data = productDto,
            Message = "Products Fetched Successfully"
        };

    }

    public async Task Insert(AddProductDto productdto, MemoryStream imgdata)
    {
        var product = _mapper.Map<Product>(productdto);
        product.ImageData = imgdata.ToArray();
        await _unitOfWork.ProductRepo.Insert(product);
        _unitOfWork.commit();
    }
}
