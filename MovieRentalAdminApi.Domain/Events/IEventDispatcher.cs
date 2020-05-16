using System.Threading.Tasks;

namespace MovieRentalAdminApi.Domain.Events
{
    public interface IEventDispatcher
    {
        Task Dispatch<TEvent>(TEvent eventToDispatch) where TEvent : IDomainEvent;
    }
}