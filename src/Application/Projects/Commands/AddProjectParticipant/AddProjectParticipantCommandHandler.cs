using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities.Projects;

namespace CleanArchitecture.Application.Projects.Commands.AddProjectParticipant
{
    public class AddProjectParticipantCommandHandler : IRequestHandler<AddProjectParticipantCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;

        public AddProjectParticipantCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task<int> Handle(AddProjectParticipantCommand request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.FindAsync(request.ProjectId);
            if (project == null)
            {
                throw new NotFoundException(nameof(Project), request.ProjectId);
            }

            var userId = await _identityService.GetUserByEmailAsync(request.ParticipantEmail);
            if (string.IsNullOrEmpty(userId))
            {
                throw new NotFoundException("ApplicationUser", request.ParticipantEmail);
            }

            var owner = await _context.Participants.FindAsync(project.OwnerId);
            if (owner == null)
            {
                throw new NotFoundException(nameof(owner), project.OwnerId);
            }

            if (owner.UserId != _currentUserService.UserId)
            {
                throw new BadRequestException($"Project owners can add/remove participants to the project.");
            }



            var participant = await _context.Participants.SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);
            project.AddParticipant(participant);

            await _context.SaveChangesAsync(cancellationToken);

            return participant.Id;
        }
    }
}