using Ecommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.IServices;

public interface IProductService
{

    Task<GenericResponseDto<IEnumerable<ProductDto>>> GetAllProductsAsync();

    Task Insert(AddProductDto product, MemoryStream imgdata);
}
