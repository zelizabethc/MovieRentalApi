using MovieRentalAdminApi.Domain.Entities;

namespace MovieRentalAdminApi.Domain.Events
{
    public class MovieUpdated : IDomainEvent
    {
        public MovieEntity Movie { get; set; }
        public string UserName { get; set; }
    }
}
