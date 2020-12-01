using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities.Projects;

namespace CleanArchitecture.Application.Projects.Queries.GetProjectIssuesList
{
    public class GetProjectIssuesListQueryHandler : IRequestHandler<GetProjectIssuesListQuery, ProjectIssuesListVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProjectIssuesListQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectIssuesListVm> Handle(GetProjectIssuesListQuery request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects
                .Include(x => x.Participants)
                .Include(x => x.Issues).ThenInclude(x => x.Assignee)
                .SingleOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken);

            if (project == null)
            {
                throw new NotFoundException(nameof(Project), request.ProjectId);
            }

            var issues = project.Issues.ToList();

            if (!string.IsNullOrEmpty(request.Title))
            {
                issues = issues.Where(x => x.Title.Contains(request.Title)).ToList();
            }

            if (!string.IsNullOrEmpty(request.Assignee))
            {
                issues = issues.Where(x => x.Assignee.FirstName.Contains(request.Assignee) || x.Assignee.LastName.Contains(request.Assignee)).ToList();
            }

            if (request.IssueType != null)
            {
                issues = issues.Where(x => x.Type == request.IssueType).ToList();
            }


            var projectIssueDtos = _mapper.Map<IList<ProjectIssueDto>>(issues);
            var vm = new ProjectIssuesListVm
            {
                Issues = projectIssueDtos,
                Count = issues.Count
            };

            return vm;
        }
    }
}