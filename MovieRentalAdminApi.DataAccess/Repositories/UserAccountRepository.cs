using Microsoft.EntityFrameworkCore;
using MovieRentalAdminApi.Domain.Entities;
using MovieRentalAdminApi.Domain.Events;
using MovieRentalAdminApi.Domain.Interfaces;
using System.Threading.Tasks;

namespace MovieRentalAdminApi.DataAccess.Repositories
{
    public class UserAccountRepository : Repository<UserAccountEntity>, IUserAccountRepository
    {
        public UserAccountRepository(MovieRentalDbContext context, IEventDispatcher domainEventsDispatcher) : base(context, domainEventsDispatcher)
        {
        }
        public async Task CreateUser(UserAccountEntity accountEntity)
        {
            Create(accountEntity);
            await SaveAsync();
        }
        public async Task<UserAccountEntity> GetByUserName(string userName)
        {
            return await FindByCondition(p => p.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<UserAccountEntity> GetByUserNameAndPassword(string userName, string password)
        {
            return await FindByCondition(p => p.UserName == userName && p.Password == password).FirstOrDefaultAsync();
        }
    }
}
