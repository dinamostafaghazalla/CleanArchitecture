using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities.Issues;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Issues.Queries.GetIssueDetail
{
    public class GetIssueDetailQueryHandler : MediatR.IRequestHandler<GetIssueDetailQuery, IssueDetailVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetIssueDetailQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IssueDetailVm> Handle(GetIssueDetailQuery request, CancellationToken cancellationToken)
        {
            var vm = await _context.Issues
                .ProjectTo<IssueDetailVm>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (vm == null)
            {
                throw new NotFoundException(nameof(Issue), request.Id);
            }

            return vm;
        }
    }
}
