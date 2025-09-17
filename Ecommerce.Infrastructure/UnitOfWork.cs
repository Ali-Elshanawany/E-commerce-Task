using Ecommerce.Application;
using Ecommerce.Application.IRepositories;
using Ecommerce.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure;

public class UnitOfWork : IUnitOfWork
{

    private ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IProductRepo ProductRepo { get { return new ProductRepo(_context); } }

    public int commit()
    {
        return _context.SaveChanges();
    }
}
