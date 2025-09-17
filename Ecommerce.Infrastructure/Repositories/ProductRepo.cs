using Ecommerce.Application.IRepositories;
using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories;

public class ProductRepo : Repo<Product>, IProductRepo
{
    private readonly ApplicationDbContext _context;

    public ProductRepo(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
}
