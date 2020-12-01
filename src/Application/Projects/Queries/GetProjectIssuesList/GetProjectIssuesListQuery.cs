using MediatR;
using CleanArchitecture.Domain.Entities.Issues;

namespace CleanArchitecture.Application.Projects.Queries.GetProjectIssuesList
{
    public class GetProjectIssuesListQuery : IRequest<ProjectIssuesListVm>
    {
        public int ProjectId { get; set; }
        public string Assignee { get; set; }
        public string Title { get; set; }
        public string Parent { get; set; }
        public IssueType? IssueType { get; set; }
    }
}