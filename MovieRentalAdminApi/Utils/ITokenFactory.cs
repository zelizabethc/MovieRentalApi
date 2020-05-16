using MovieRentalAdminApi.Domain.Entities;

namespace MovieRentalAdminApi.Utils
{
    public interface ITokenFactory
    {
        string UserIdClaim { get; }
        string RoleClaim { get; }
        string GenerateToken(string userId, Role role);
        string GetUser();
        string GetRole();
    }
}