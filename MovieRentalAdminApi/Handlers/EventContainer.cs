using Microsoft.Extensions.DependencyInjection;
using MovieRentalAdminApi.Domain.Events;
using System;
using System.Threading.Tasks;

namespace MovieRentalAdminApi.Handlers
{
    public class EventContainer : IEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        public EventContainer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Dispatch<TEvent>(TEvent eventToDispatch) where TEvent : IDomainEvent
        {
            if (eventToDispatch.GetType() == typeof(MovieUpdated))
            {
                var domainHandler = _serviceProvider.GetServices<IDomainHandler<MovieUpdated>>();
                foreach (var handler in domainHandler)
                {
                    await handler.Handle(eventToDispatch as MovieUpdated);
                }
            }
            if (eventToDispatch.GetType() == typeof(MovieAction))
            {
                var domainHandler = _serviceProvider.GetServices<IDomainHandler<MovieAction>>();
                foreach (var handler in domainHandler)
                {
                    await handler.Handle(eventToDispatch as MovieAction);
                }
            }
        }
    }
}
