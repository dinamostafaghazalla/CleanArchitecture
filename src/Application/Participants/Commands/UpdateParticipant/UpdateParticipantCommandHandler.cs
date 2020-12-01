using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities.Participants;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Exceptions;

namespace CleanArchitecture.Application.Participants.Commands.UpdateParticipant
{
    public class UpdateParticipantCommandHandler : IRequestHandler<UpdateParticipantCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public UpdateParticipantCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMediator mediator, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateParticipantCommand request, CancellationToken cancellationToken)
        {
            var participant = await _context.Participants.FindAsync(request.Id);

            if (participant == null)
            {
                throw new NotFoundException($"{nameof(Participant)} {request.Id} not found");
            }

            participant.SetFirstName(request.FirstName)
                .SetLastName(request.LastName)
                .SetTitle(request.Title);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}