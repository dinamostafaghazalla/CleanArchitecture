using System.Collections.Generic;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities.Issues;
using CleanArchitecture.Domain.Entities.Participants;
using CleanArchitecture.Domain.Exceptions;

namespace CleanArchitecture.Domain.Entities.Projects
{
    public class Project : AuditableEntity
    {
        private Project()
        {
        }

        public Project(Participant owner, string name, string key)
        {
            SetOwner(owner)
                .SetName(name)
                .SetKey(key);
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Key { get; private set; }

        public int OwnerId { get; private set; }
        public Participant Owner { get; private set; }

        public ICollection<Participant> Participants { get; private set; } = new List<Participant>();
        public ICollection<Issue> Issues { get; private set; } = new List<Issue>();


        public Project AddIssue(Issue issue)
        {
            if (issue == null)
                throw new DomainException($"{nameof(issue)} can't be null");

            if (Issues.Contains(issue))
                throw new DuplicateException($"{nameof(issue)} is already exist");

            Issues.Add(issue);
            return this;
        }

        public Project RemoveIssue(Issue issue)
        {
            if (issue == null)
                throw new DomainException($"{nameof(issue)} can't be null");

            if (Issues.Contains(issue))
                Issues.Remove(issue);

            return this;
        }

        public Project AddParticipant(Participant participant)
        {
            if (participant == null)
                throw new DomainException($"{nameof(participant)} can't be null");

            if (Participants.Contains(participant))
                throw new DuplicateException($"{nameof(participant)} is already exist");

            Participants.Add(participant);
            return this;
        }

        public Project RemoveParticipant(Participant participant)
        {
            if (participant == null)
                throw new DomainException($"{nameof(participant)} can't be null");

            if (Participants.Contains(participant))
                Participants.Remove(participant);
            return this;
        }

        public Project SetOwner(Participant participant)
        {
            Owner = participant ?? throw new DomainException($"{nameof(Owner)} can't be null");
            AddParticipant(participant);
            return this;
        }

        public Project SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new DomainException($"{nameof(name)} can't be null or empty");

            Name = name;
            return this;
        }

        public Project SetKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new DomainException($"{nameof(key)} can't be null or empty");

            Key = key.ToUpper();
            return this;
        }
    }
}