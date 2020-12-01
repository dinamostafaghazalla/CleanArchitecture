using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities.Issues;

namespace CleanArchitecture.Domain.Events.Issues
{
    public class IssueStatusUpdatedEvent : DomainEvent
    {
        public IssueStatusUpdatedEvent(Issue item)
        {
            Item = item;
        }

        public Issue Item { get; }
    }
}