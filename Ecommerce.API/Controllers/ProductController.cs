using Ecommerce.Application.DTOs;
using Ecommerce.Application.IServices;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecommerce.API.Controllers;

//[Authorize(AuthenticationSchemes = "Bearer")]
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{

    private readonly IProductService _ProductService;

    public ProductController(IProductService productService)
    {
        _ProductService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> getProducts()
    {

        //var products = generateProduct();
        //var res = new GenericResponseDto<ProductDto[]>()
        //{
        //    Message = "Fetch Products",
        //    Status = 200,
        //    Data = products
        //};
        var res = await _ProductService.GetAllProductsAsync();



        return Ok(res);
    }

    [HttpPost]
    public async Task<IActionResult> InsertProduct([FromForm] AddProductDto productDto)
    {
        try
        {
            using var dataStream = new MemoryStream();

            await productDto.ImageData.CopyToAsync(dataStream);

            await _ProductService.Insert(productDto, dataStream);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }



    private ProductDto[] generateProduct()
    {
        var products = new ProductDto[10];

        for (int i = 0; i < products.Length; i++)
        {
            products[i] = new ProductDto
            {
                Id = i,
                Name = $"Name:{i}",
                Category = $"Cat:{i}",
                MinQuantity = i * 10 + 5,  // use the value you actually want
                Price = i * 100 + 1,
                DiscountRate = i
            };
        }

        return products;
    }



}
