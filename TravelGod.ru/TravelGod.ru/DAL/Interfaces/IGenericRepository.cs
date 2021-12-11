using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using TravelGod.ru.Infrastructure;

namespace TravelGod.ru.DAL.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Create(TEntity item);
        TEntity FindById(int id);
        Task<TEntity> FindByIdAsync(int id);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter = null,
                               Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                               Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null,
                                          Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include =
                                              null,
                                          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                 Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                 Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
                                            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include =
                                                null,
                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        void Remove(TEntity item);
        void Update(TEntity item);

        Task<PaginatedList<TEntity>> GetPaginatedListAsync(int pageSize,
                                                           int pageIndex,
                                                           Expression<Func<TEntity, bool>> filter = null,
                                                           Func<IQueryable<TEntity>,
                                                                   IIncludableQueryable<TEntity, object>>
                                                               include = null,
                                                           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>
                                                               orderBy = null);
    }
}