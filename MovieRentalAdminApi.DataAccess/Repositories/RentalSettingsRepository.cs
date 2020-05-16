using Microsoft.EntityFrameworkCore;
using MovieRentalAdminApi.Domain.Entities;
using MovieRentalAdminApi.Domain.Events;
using MovieRentalAdminApi.Domain.Interfaces;
using System.Threading.Tasks;

namespace MovieRentalAdminApi.DataAccess.Repositories
{
    public class RentalSettingsRepository : Repository<RentalSettingsEntity>, IRentalSettingsRepository
    {
        public RentalSettingsRepository(MovieRentalDbContext context, IEventDispatcher domainEventsDispatcher) : base(context, domainEventsDispatcher)
        {
        }
        public async Task<RentalSettingsEntity> GetActiveSetting()
        {
            return await FindByCondition(p => p.Status == 1).FirstOrDefaultAsync();
        }
    }
}
