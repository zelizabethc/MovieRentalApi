using MovieRentalAdminApi.Domain.Entities;
using MovieRentalAdminApi.Domain.Events;
using MovieRentalAdminApi.Domain.Interfaces;
using MovieRentalAdminApi.Utils;
using System.Threading.Tasks;

namespace MovieRentalAdminApi.Handlers 
{
    public class MovieActionHandler : IDomainHandler<MovieAction>
    {
        private readonly IRepository<MovieActionLogEntity> _movieActionLogRepository;
        private readonly IRepository<RentedTrackingEntity> _rentedTrackingLogRepository;
        private readonly ITokenFactory _tokenFactory;

        public MovieActionHandler(IRepository<MovieActionLogEntity> movieActionLogRepository, IRepository<RentedTrackingEntity> rentedTrackingLogRepository, ITokenFactory tokenFactory)
        {
            _movieActionLogRepository = movieActionLogRepository;
            _rentedTrackingLogRepository = rentedTrackingLogRepository;
            _tokenFactory = tokenFactory;
        }

        public async Task Handle(MovieAction @event)
        {
            _movieActionLogRepository.Create(new MovieActionLogEntity
            {
                Quantity = @event.Quantity,
                IdMovie = @event.Movie.Id,
                Action = @event.Action,
                UserName = _tokenFactory.GetUser()
            });

            if(@event.Action.Equals("RENT"))
                _rentedTrackingLogRepository.Create(new RentedTrackingEntity
                {
                    IdMovie = @event.Movie.Id,
                    Quantity = @event.Quantity,
                    DaysForRent = @event.DaysForRent,
                    Penalty = @event.Penalty,
                    DueDate = @event.DueDate
                });
        }
    }
}
