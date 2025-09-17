using Ecommerce.Application.Enums;
using Ecommerce.Application.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories;

public class Repo<T> : IRepo<T> where T : class
{
    private ApplicationDbContext _context;

    public Repo(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>>? criteria, int? skip,
        int? take, string[] includes = null, Expression<Func<T, object>> orderBy = null, string orderByDirection = "ASC")
    {
        IQueryable<T> query;
        if (criteria != null)
            query = _context.Set<T>().Where(criteria);
        else
            query = _context.Set<T>();

        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);

        if (orderBy != null)
        {
            if (orderByDirection == OrderBy.Ascending)
                query = query.OrderBy(orderBy);
            else
                query = query.OrderByDescending(orderBy);
        }
        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);


        return await query.ToListAsync();
    }

    public async Task Insert(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }
}
