using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities.Issues;
using CleanArchitecture.Domain.Entities.Projects;
using CleanArchitecture.Domain.Exceptions;
using System.Collections.Generic;

namespace CleanArchitecture.Domain.Entities.Participants
{
    public class Participant : AuditableEntity
    {
        private Participant()
        {
        }

        public Participant(string userId, string firstName, string lastName)
        {
            SetUserId(userId)
                .SetFirstName(firstName)
                .SetLastName(lastName);
        }

        public int Id { get; private set; }
        public string UserId { get; private set; }
        public string LastName { get; private set; }
        public string FirstName { get; private set; }
        public string Title { get; private set; }
        public ICollection<Project> OwnProjects { get; private set; } = new List<Project>();
        public ICollection<Project> Projects { get; private set; } = new List<Project>();
        public ICollection<Issue> OwnIssues { get; private set; } = new List<Issue>();
        public ICollection<Issue> AssignedIssues { get; private set; } = new List<Issue>();

        public Participant SetFirstName(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw new DomainException($"{nameof(firstName)} can't be null or empty.");
            }

            FirstName = firstName;
            return this;
        }

        public Participant SetLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
            {
                throw new DomainException($"{nameof(lastName)} can't be null or empty.");
            }

            LastName = lastName;
            return this;
        }

        public Participant SetTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new DomainException($"{nameof(title)} can't be null or empty.");
            }

            Title = title;
            return this;
        }

        public Participant SetUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new DomainException($"{nameof(userId)} can't be null or empty.");
            }

            UserId = userId;
            return this;
        }

        public Participant AddProject(Project project)
        {
            if (Projects.Contains(project))
            {
                throw new DuplicateException($"{nameof(project)} already exist");
            }

            Projects.Add(project);
            return this;
        }

        public Participant AssignIssue(Issue issue)
        {
            if (issue == null)
            {
                throw new Exceptions.DomainException($"{nameof(issue)} can't be null");
            }

            if (AssignedIssues.Contains(issue))
            {
                throw new DuplicateException($"{nameof(issue)} is already exist");
            }

            AssignedIssues.Add(issue);
            return this;
        }
    }
}