using MediatR;

namespace CleanArchitecture.Application.Issues.Queries.GetIssueDetail
{
    public class GetIssueDetailQuery : IRequest<IssueDetailVm>
    {
        public int Id { get; set; }
    }
}
