using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Application.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities.Projects;
using CleanArchitecture.Application.Common.Exceptions;

namespace CleanArchitecture.Application.Projects.Queries.GetProjectParticipantsList
{
    public class GetProjectParticipantsListQueryHandler : IRequestHandler<GetProjectParticipantsListQuery, ProjectParticipantsListVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private IIdentityService _identityService;

        public GetProjectParticipantsListQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task<ProjectParticipantsListVm> Handle(GetProjectParticipantsListQuery request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.Include(x => x.Participants)
                .SingleOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken);
            if (project == null)
            {
                throw new NotFoundException(nameof(Project), request.ProjectId);
            }

            var participant = await _context.Participants.SingleOrDefaultAsync(x => x.UserId == _currentUserService.UserId, cancellationToken);

            var isParticipant = project.Participants.Contains(participant);
            if (!isParticipant)
            {
                throw new BadRequestException($"Project participants can list all other participants in a project");
            }

            var participants = project.Participants.ToList();
            
            var projectParticipantDto = _mapper.Map<IList<ProjectParticipantDto>>(participants);

            var vm = new ProjectParticipantsListVm
            {
                Participants = projectParticipantDto,
                Count = participants.Count
            };

            return vm;
        }
    }
}