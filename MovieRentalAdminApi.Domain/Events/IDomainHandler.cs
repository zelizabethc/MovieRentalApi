using System.Threading.Tasks;

namespace MovieRentalAdminApi.Domain.Events
{
    public interface IDomainHandler<T> where T : IDomainEvent
    {
        Task Handle(T @event);
    }
}
