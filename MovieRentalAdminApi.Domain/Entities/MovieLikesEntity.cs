using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieRentalAdminApi.Domain.Entities
{
    public class MovieLikesEntity : Entity
    {
        [Key]
        public int UserAccountId { get; set; }
        public UserAccountEntity UserAccount { get; set; }
        [Key]
        public int MovieId { get; set; }
        public MovieEntity Movie { get; set; }

        public static List<MovieLikesEntity> CreateDumpData()
        {
            return new List<MovieLikesEntity>
            {
                new MovieLikesEntity()
                {
                    MovieId = 1,
                    UserAccountId = 2
                },
                new MovieLikesEntity()
                {
                    MovieId = 1,
                    UserAccountId = 1
                },
                new MovieLikesEntity()
                {
                    MovieId = 2,
                    UserAccountId = 2
                }
            };
        }
    }
}