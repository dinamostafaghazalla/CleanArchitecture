using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Models;
using MediatR;

namespace CleanArchitecture.Application.Issues.Queries.GetIssuesWithPagination
{
    public class GetIssuesWithPaginationQueryHandler : IRequestHandler<GetIssuesWithPaginationQuery, PaginatedList<IssueDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetIssuesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<IssueDto>> Handle(GetIssuesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _context.Issues
                    .Where(x => x.ProjectId == request.ProjectId)
                    .OrderBy(x => x.Title)
                    .ProjectTo<IssueDto>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync(request.PageNumber, request.PageSize)
                ;
        }
    }
}