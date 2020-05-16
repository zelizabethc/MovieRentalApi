using MovieRentalAdminApi.Domain.Entities;
using MovieRentalAdminApi.Domain.Interfaces;
using System.Threading.Tasks;

namespace MovieRentalAdminApi.Domain.Interfaces
{
    public interface IUserAccountRepository : IRepository<UserAccountEntity>
    {
        Task<UserAccountEntity> GetByUserName(string userName);
        Task<UserAccountEntity> GetByUserNameAndPassword(string userName, string password);
        Task CreateUser(UserAccountEntity accountEntity);
    }
}