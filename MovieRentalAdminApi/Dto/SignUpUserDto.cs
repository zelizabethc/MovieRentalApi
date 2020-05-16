using MovieRentalAdminApi.Domain.Entities;

namespace MovieRentalAdminApi.Dto
{
    public class SignUpUserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}