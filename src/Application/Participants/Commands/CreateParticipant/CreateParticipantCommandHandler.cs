using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities.Participants;
using CleanArchitecture.Domain.Entities.Projects;
using CleanArchitecture.Domain.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Exceptions;

namespace CleanArchitecture.Application.Participants.Commands.CreateParticipant
{
    public class CreateParticipantCommandHandler : IRequestHandler<CreateParticipantCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public CreateParticipantCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<int> Handle(CreateParticipantCommand request, CancellationToken cancellationToken)
        {
            var userId = await _identityService.GetUserByEmailAsync(request.Email);
            var participant = new Participant(userId, request.FirstName, request.LastName);
            
            participant.SetTitle(request.Title);
            
            if (request.ProjectId != null)
            {
                var project = await _context.Projects.FindAsync(request.ProjectId);
                if (project == null)
                {
                    throw new NotFoundException($"{nameof(Project)} {request.ProjectId} not found");
                }

                participant.AddProject(project);
            }


            await _context.Participants.AddAsync(participant, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return participant.Id;
        }
    }
}