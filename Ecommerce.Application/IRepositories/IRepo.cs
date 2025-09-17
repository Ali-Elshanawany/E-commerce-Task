using Ecommerce.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.IRepositories;

public interface IRepo<T> where T : class
{
    Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>>? criteria, int? skip, int? take, string[] includes = null,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending);

    public Task Insert(T entity);


}
