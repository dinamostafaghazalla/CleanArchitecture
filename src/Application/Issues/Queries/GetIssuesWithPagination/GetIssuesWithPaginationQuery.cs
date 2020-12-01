using CleanArchitecture.Application.Common.Models;
using MediatR;

namespace CleanArchitecture.Application.Issues.Queries.GetIssuesWithPagination
{
    public class GetIssuesWithPaginationQuery : IRequest<PaginatedList<IssueDto>>
    {
        public int ProjectId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}