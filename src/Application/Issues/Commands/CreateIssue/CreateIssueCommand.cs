using CleanArchitecture.Domain.Entities.Issues;
using CleanArchitecture.Domain.Enums;
using MediatR;

namespace CleanArchitecture.Application.Issues.Commands.CreateIssue
{
    public class CreateIssueCommand : IRequest<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int? AssigneeId { get; set; }
        public IssueStatus Status { get; set; }
        public IssueType IssueType { get; set; }
        public int ProjectId { get; set; }
    }
}