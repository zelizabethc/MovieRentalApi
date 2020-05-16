using MovieRentalAdminApi.Domain.Entities;
using System.Threading.Tasks;

namespace MovieRentalAdminApi.Domain.Interfaces
{
    public interface IRentalSettingsRepository
    {
        Task<RentalSettingsEntity> GetActiveSetting();
    }
}