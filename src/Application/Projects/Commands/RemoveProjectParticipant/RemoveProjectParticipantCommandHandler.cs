using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Exceptions;

namespace CleanArchitecture.Application.Projects.Commands.RemoveProjectParticipant
{
    public class RemoveProjectParticipantCommandHandler : IRequestHandler<RemoveProjectParticipantCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;

        public RemoveProjectParticipantCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task<Unit> Handle(RemoveProjectParticipantCommand request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.Include(x => x.Participants).SingleOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken);
            if (project == null)
            {
                throw new NotFoundException($"project {request.ProjectId} not found.");
            }

            var userId = await _identityService.GetUserByEmailAsync(request.ParticipantEmail);
            if (string.IsNullOrEmpty(userId))
            {
                throw new NotFoundException($"Participant {request.ParticipantEmail} not found.");
            }

            var owner = await _context.Participants.FindAsync(project.OwnerId);
            if (owner == null)
            {
                throw new NotFoundException($"owner {project.OwnerId} not found.");
            }

            if (owner.UserId != _currentUserService.UserId)
            {
                throw new DomainException($"Project owners can add/remove participants to the project.");
            }


            var participant = await _context.Participants.SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);
            project.RemoveParticipant(participant);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}