using MediatR;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities.Participants;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Participants.Commands.DeleteParticipant
{
    public class DeleteParticipantCommand : IRequest
    {
        public int Id { get; set; }

        public class DeleteParticipantCommandHandler : IRequestHandler<DeleteParticipantCommand>
        {
            private readonly IApplicationDbContext _context;
            private readonly IIdentityService _identityService;
            private readonly ICurrentUserService _currentUser;

            public DeleteParticipantCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IIdentityService identityService)
            {
                _context = context;
                _currentUser = currentUser;
                _identityService = identityService;
            }

            public async Task<Unit> Handle(DeleteParticipantCommand request, CancellationToken cancellationToken)
            {
                var entity = await _context.Participants
                    .FindAsync(request.Id);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(Participant), request.Id);
                }

                if (entity.UserId == _currentUser.UserId)
                {
                    throw new BadRequestException("Participants cannot delete their own account.");
                }

                if (entity.UserId != null)
                {
                    await _identityService.DeleteUserAsync(entity.UserId);
                }

                // TODO: Update this logic, this will only work if the participant has no associated territories or orders.Emp

                _context.Participants.Remove(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}