using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Infrastructure;
using TravelGod.ru.Models;
using TravelGod.ru.Pages.Admin.ViewModels;

namespace TravelGod.ru.DAL
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context) : base(context)
        {
        }

        public new void Create(User item)
        {
            item.Avatar = new Avatar
            {
                File = Context.Files.Find(1)
            };
            item.Avatar.File ??= new File
            {
                Id = 1,
                Name = "default-avatar.png",
                Path = "CustomFiles/Avatars/default-avatar.png"
            };
            base.Create(item);
        }

        public IEnumerable<User> Get(UserFilter filter,
                                     Func<IQueryable<User>, IIncludableQueryable<User, object>> include = null,
                                     Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null)
        {
            return ApplyFilter(filter, include, orderBy).ToList();
        }

        public async Task<IEnumerable<User>> GetAsync(UserFilter filter,
                                                      Func<IQueryable<User>, IIncludableQueryable<User, object>>
                                                          include =
                                                          null,
                                                      Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null)
        {
            return await ApplyFilter(filter, include, orderBy).ToListAsync();
        }

        public async Task<PaginatedList<User>> GetPaginatedListAsync(int pageSize,
                                                                     int pageIndex = 1,
                                                                     UserFilter filter = null,
                                                                     Func<IQueryable<User>,
                                                                             IIncludableQueryable<User, object>>
                                                                         include =
                                                                         null,
                                                                     Func<IQueryable<User>, IOrderedQueryable<User>>
                                                                         orderBy =
                                                                         null)
        {
            var source = ApplyFilter(filter, include, orderBy);
            return await PaginatedList<User>.CreateAsync(source, pageIndex, pageSize);
        }

        private IQueryable<User> ApplyFilter(UserFilter filter,
                                             Func<IQueryable<User>, IIncludableQueryable<User, object>> include = null,
                                             Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = null)
        {
            var users = Apply(null, include, orderBy);

            if (filter is null)
            {
                return users;
            }

            if (!string.IsNullOrEmpty(filter.Name))
            {
                users = users.Where(u => u.Login.ToLower()
                                          .Contains(filter.Name) ||
                                         (u.LastName + ' ' + u.FirstName + ' ' + u.Patronymic).ToLower()
                                         .Contains(filter.Name));
            }

            users = users.Where(u => u.Role == filter.Role &&
                                     u.Status == filter.Status);
            return users;
        }
    }
}