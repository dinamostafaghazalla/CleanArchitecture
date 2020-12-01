using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities.Projects;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Projects.Queries.GetProjectDetail
{
    public class GetProjectDetailQueryHandler : MediatR.IRequestHandler<GetProjectDetailQuery, ProjectDetailVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProjectDetailQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectDetailVm> Handle(GetProjectDetailQuery request, CancellationToken cancellationToken)
        {
            var vm = await _context.Projects
                .ProjectTo<ProjectDetailVm>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (vm == null)
            {
                throw new NotFoundException(nameof(Project), request.Id);
            }

            return vm;
        }
    }
}
