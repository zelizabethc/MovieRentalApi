using MovieRentalAdminApi.Domain.Entities;
using MovieRentalAdminApi.Domain.Events;
using MovieRentalAdminApi.Domain.Interfaces;
using MovieRentalAdminApi.Utils;
using System.Threading.Tasks;

namespace MovieRentalAdminApi.Handlers
{
    public class MovieUpdatedHandler : IDomainHandler<MovieUpdated>
    {
        private readonly IRepository<MovieUpdateLogEntity> _movieUpdateLogRepository;
        private readonly ITokenFactory _tokenFactory;

        public MovieUpdatedHandler(IRepository<MovieUpdateLogEntity> movieUpdateLogRepository, ITokenFactory tokenFactory)
        {
            _movieUpdateLogRepository = movieUpdateLogRepository;
            _tokenFactory = tokenFactory;
        }

        public async Task Handle(MovieUpdated @event)
        {
            _movieUpdateLogRepository.Create(new MovieUpdateLogEntity
            {
                IdMovie = @event.Movie.Id,
                Title = @event.Movie.Title,
                RentalPrice = @event.Movie.RentalPrice,
                SalePrice = @event.Movie.SalePrice,
                UserName = _tokenFactory.GetUser()
            });
        }
    }
}
