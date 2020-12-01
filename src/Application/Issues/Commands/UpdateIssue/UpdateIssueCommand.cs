using CleanArchitecture.Domain.Enums;
using MediatR;

namespace CleanArchitecture.Application.Issues.Commands.UpdateIssue
{
    public class UpdateIssueCommand : IRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string IssueId { get; set; }
        public string Description { get; set; }
        public int ReporterId { get; set; }
        public int? AssigneeId { get; set; }
        public int? ProjectId { get; set; }
        public IssueStatus Status { get; set; }
    }
}