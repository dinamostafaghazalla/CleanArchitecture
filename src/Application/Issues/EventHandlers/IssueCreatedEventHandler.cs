using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.Events.Issues;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Issues.EventHandlers
{
    public class IssueCreatedEventHandler : INotificationHandler<DomainEventNotification<IssueCreatedEvent>>
    {
        private readonly ILogger<IssueStatusUpdatedEventHandler> _logger;

        public IssueCreatedEventHandler(ILogger<IssueStatusUpdatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DomainEventNotification<IssueCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
