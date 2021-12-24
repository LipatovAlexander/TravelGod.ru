using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;
using TravelGod.ru.ViewModels;

namespace TravelGod.ru.DAL.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        new void Create(User item);

        void Create(SignUpModel item);

        IEnumerable<User> Get(UserFilter options,
                              Func<IQueryable<User>, IIncludableQueryable<User, object>> include = null,
                              Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null);

        Task<IEnumerable<User>> GetAsync(UserFilter filter,
                                         Func<IQueryable<User>, IIncludableQueryable<User, object>> include = null,
                                         Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null);

        Task<PaginatedList<User>> GetPaginatedListAsync(int pageSize,
                                                        int pageIndex,
                                                        UserFilter filter,
                                                        Func<IQueryable<User>, IIncludableQueryable<User, object>>
                                                            include = null,
                                                        Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null);
    }
}