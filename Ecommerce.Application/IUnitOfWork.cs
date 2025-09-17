using Ecommerce.Application.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application;

public interface IUnitOfWork
{

    IProductRepo ProductRepo { get; }

    int commit();

}
