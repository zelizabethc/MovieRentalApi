using MovieRentalAdminApi.CrossCutting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRentalAdminApi.Domain.Entities
{
    public class UserAccountEntity : Entity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; protected set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public ICollection<MovieLikesEntity> LikedMovies { get; set; }

        public UserAccountEntity Create(string userName, string password, Role role)
        {
            if (string.IsNullOrEmpty(userName.Trim()) || string.IsNullOrEmpty(password.Trim()))
                return null;
            return new UserAccountEntity()
            {
                Password = password,
                Role = role,
                UserName = userName
            };
        }

        public static List<UserAccountEntity> CreateDumpData()
        {
            return new List<UserAccountEntity>
            {
                new UserAccountEntity()
                {
                    UserName = "admin",
                    Password = "admin".ComputeSha256Hash(),
                    Role = Role.Admin
                },
                new UserAccountEntity()
                {
                    UserName = "diana.campos",
                    Password = "12345".ComputeSha256Hash(),
                    Role = Role.Customer
                }
            };
        }
    }

    public enum Role
    {
        Admin = 0,
        Customer = 1
    }
}
