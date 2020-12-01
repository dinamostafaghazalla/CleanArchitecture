using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities.Issues;

namespace CleanArchitecture.Domain.Events.Issues
{
    public class IssueCreatedEvent : DomainEvent
    {
        public IssueCreatedEvent(Issue item)
        {
            Item = item;
        }

        public Issue Item { get; }
    }
}
