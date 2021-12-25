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
    public interface ITripRepository : IGenericRepository<Trip>
    {
        Task AddUserAsync(int tripId, User user);
        Task AddUserAsync(Trip trip, User user);
        void Create(Trip trip, User creator, bool createChat);
        Task CreateAsync(Trip trip, User creator, bool createChat);

        IEnumerable<Trip> Get(TripFilter filter,
                              Func<IQueryable<Trip>, IIncludableQueryable<Trip, object>> include = null,
                              Func<IQueryable<Trip>, IOrderedQueryable<Trip>> orderBy = null);

        Task<IEnumerable<Trip>> GetAsync(TripFilter filter,
                                         Func<IQueryable<Trip>, IIncludableQueryable<Trip, object>> include = null,
                                         Func<IQueryable<Trip>, IOrderedQueryable<Trip>> orderBy = null);

        Task<PaginatedList<Trip>> GetPaginatedListAsync(int pageSize,
                                                        int pageIndex,
                                                        TripFilter filter,
                                                        Func<IQueryable<Trip>, IIncludableQueryable<Trip, object>>
                                                            include = null,
                                                        Func<IQueryable<Trip>, IOrderedQueryable<Trip>> orderBy = null);

        Task UpdateAsync(Trip trip, bool createChat);
        new void Update(Trip trip);
    }
}