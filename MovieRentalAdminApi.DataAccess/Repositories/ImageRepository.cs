using MovieRentalAdminApi.Domain.Entities;
using MovieRentalAdminApi.Domain.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieRentalAdminApi.DataAccess.Repositories
{
    public class ImageRepository : Repository<ImageEntity>, IImageRepository
    {
        public ImageRepository(MovieRentalDbContext context, IEventDispatcher domainEventsDispatcher) : base(context, domainEventsDispatcher)
        {
        }

        public void RemoveRange(IEnumerable<ImageEntity> rangeToRemove)
        {
            _dbSet.RemoveRange(rangeToRemove);
        }

        public void AddRange(IEnumerable<ImageEntity> rangeToAdd)
        {
            _dbSet.AddRange(rangeToAdd);
        }
    }
}
