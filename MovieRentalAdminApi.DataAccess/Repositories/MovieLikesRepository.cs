using MovieRentalAdminApi.Domain.Entities;
using MovieRentalAdminApi.Domain.Events;
using MovieRentalAdminApi.Domain.Interfaces;
using System.Collections.Generic;

namespace MovieRentalAdminApi.DataAccess.Repositories
{
    public class MovieLikesRepository : Repository<MovieLikesEntity>, IMovieLikesRepository
    {
        public MovieLikesRepository(MovieRentalDbContext context, IEventDispatcher domainEventsDispatcher) : base(context, domainEventsDispatcher)
        {
        }

        public void RemoveRange(IEnumerable<MovieLikesEntity> rangeToRemove)
        {
            _dbSet.RemoveRange(rangeToRemove);
        }

        public void AddRange(IEnumerable<MovieLikesEntity> rangeToAdd)
        {
            _dbSet.AddRange(rangeToAdd);
        }
    }
}
