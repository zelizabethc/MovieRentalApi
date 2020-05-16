using MovieRentalAdminApi.Domain.Entities;
using System.Collections.Generic;

namespace MovieRentalAdminApi.DataAccess.Repositories
{
    public interface IImageRepository
    {
        void RemoveRange(IEnumerable<ImageEntity> rangeToRemove);
        void AddRange(IEnumerable<ImageEntity> rangeToAdd);
    }
}