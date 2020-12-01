using System.Collections.Generic;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities.Participants;
using CleanArchitecture.Domain.Entities.Projects;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Exceptions;

namespace CleanArchitecture.Domain.Entities.Issues
{
    public class Issue : AuditableEntity, IHasDomainEvent
    {
        private Issue()
        {
        }

        public Issue(Project project, Participant reporter, IssueType type, string title, IssueStatus status = IssueStatus.Todo)
        {
            SetProject(project)
                .SetIssueId(string.Empty)
                .SetReporter(reporter)
                .SetType(type)
                .SetTitle(title)
                .SetStatus(status)
                ;
        }

        public int Id { get; private set; }
        public string IssueId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int ReporterId { get; private set; }
        public int? AssigneeId { get; private set; }
        public int ProjectId { get; private set; }

        public Participant Reporter { get; private set; }
        public Participant Assignee { get; private set; }
        public Project Project { get; private set; }
        public IssueStatus Status { get; private set; }
        public IssueType Type { get; private set; }
        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();

        public Issue AddDomainEvent(List<DomainEvent> domainEvents)
        {
            DomainEvents.AddRange(domainEvents);
            return this;
        }

        public Issue SetType(IssueType type)
        {
            Type = type;
            return this;
        }

        public Issue SetStatus(IssueStatus status = IssueStatus.Todo)
        {
            Status = status;
            return this;
        }

        public Issue SetAssignee(Participant participant)
        {
            Assignee = participant ?? throw new DomainException($"{nameof(participant)} can't be null");

            return this;
        }

        public Issue SetProject(Project project)
        {
            Project = project ?? throw new DomainException($"{nameof(project)} can't be null");

            return this;
        }

        public Issue SetReporter(Participant participant)
        {
            Reporter = participant ?? throw new DomainException($"{nameof(participant)} can't be null");

            return this;
        }

        public Issue SetDescription(string description)
        {
            Description = description;

            return this;
        }

        public Issue SetTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new DomainException($"{nameof(title)} can't be null or empty");
            }

            Title = title;

            return this;
        }

        public Issue SetIssueId(string issueId)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                var projectIssues = Project.Issues;
                if (projectIssues == null)
                {
                    throw new DomainException($"{nameof(projectIssues)} can't be null ");
                }

                var lastIssueId = projectIssues.Count;
                issueId = $"{Project.Key}-{lastIssueId + 1}";
            }

            IssueId = issueId;
            return this;
        }
    }
}