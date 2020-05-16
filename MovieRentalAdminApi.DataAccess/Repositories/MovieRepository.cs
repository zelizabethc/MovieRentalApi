using Microsoft.EntityFrameworkCore;
using MovieRentalAdminApi.CrossCutting.Dto;
using MovieRentalAdminApi.Domain.Entities;
using MovieRentalAdminApi.Domain.Events;
using MovieRentalAdminApi.Domain.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRentalAdminApi.DataAccess.Repositories
{
    public class MovieRepository : Repository<MovieEntity>, IMovieRepository
    {
        public MovieRepository(MovieRentalDbContext context, IEventDispatcher domainEventsDispatcher) : base(context, domainEventsDispatcher)
        {
        }
        public async Task CreateMovie(MovieEntity movie)
        {
            Create(movie);
            await SaveAsync();
        }

        public async Task<MovieEntity> GetById(int id)
        {
            return await FindByCondition(p => p.Id == id).Include(a => a.UserAccountLikes).Include(b => b.Images).FirstOrDefaultAsync();
        }

        public async Task UpdateMovie(MovieEntity movie)
        {
            Update(movie);
            await SaveAsync();
        }
        public async Task DeleteMovie(MovieEntity movie)
        {
            Delete(movie);
            await SaveAsync();
        }
        public async Task<IEnumerable<MovieEntity>> SearchByTitle(string title)
        {
            return await FindByCondition(p => p.Title.Contains(title)).ToListAsync();
        }

        public async Task<MovieEntity> GetByTitle(string title)
        {
            return await FindByCondition(p => p.Title == title).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<MovieEntity>> GetMoviesByFilters(PaginationDto pagination)
        {
            var property = TypeDescriptor.GetProperties(typeof(MovieEntity)).Find(pagination.SortBy, true);
            if(pagination.Filter == 2)
            {
                var query = pagination.Order == "Desc"
                    ? FindAll().OrderByDescending(a => property.GetValue(a))
                    : FindAll().OrderBy(a => property.GetValue(a));
                return await query
                    .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();
            }
            else
            {
                var query = pagination.Order == "Desc"
                    ? FindByCondition(p => p.Availability == pagination.Filter).OrderByDescending(a => property.GetValue(a))
                    : FindByCondition(p => p.Availability == pagination.Filter).OrderBy(a => property.GetValue(a));
                return await query
                    .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();
            }
        }
    }
}