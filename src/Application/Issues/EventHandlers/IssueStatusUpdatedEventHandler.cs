using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.Events.Issues;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Issues.EventHandlers
{
    public class IssueStatusUpdatedEventHandler : INotificationHandler<DomainEventNotification<IssueStatusUpdatedEvent>>
    {
        private readonly ILogger<IssueStatusUpdatedEventHandler> _logger;

        public IssueStatusUpdatedEventHandler(ILogger<IssueStatusUpdatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DomainEventNotification<IssueStatusUpdatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
