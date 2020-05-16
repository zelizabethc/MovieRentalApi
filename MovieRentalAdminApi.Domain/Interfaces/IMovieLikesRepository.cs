using MovieRentalAdminApi.Domain.Entities;
using System.Collections.Generic;

namespace MovieRentalAdminApi.Domain.Interfaces
{
    public interface IMovieLikesRepository : IRepository<MovieLikesEntity>
    {
        void RemoveRange(IEnumerable<MovieLikesEntity> rangeToRemove);
        void AddRange(IEnumerable<MovieLikesEntity> rangeToAdd);
    }
}