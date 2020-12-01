using System.Linq;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Projects.Queries.GetProjectParticipantsList;
using CleanArchitecture.Domain.Entities.Issues;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Exceptions;

namespace CleanArchitecture.Application.Issues.Commands.DeleteIssue
{
    public class DeleteIssueCommandHandler : IRequestHandler<DeleteIssueCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public DeleteIssueCommandHandler(IApplicationDbContext context, IMapper mapper, IMediator mediator, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(DeleteIssueCommand request, CancellationToken cancellationToken)
        {
            var issue = await _context.Issues.FindAsync(request.Id);

            if (issue == null)
            {
                throw new NotFoundException(nameof(Issue), request.Id);
            }

            var participantsListQuery = new GetProjectParticipantsListQuery { ProjectId = issue.ProjectId };

            var projectParticipantsListVm = await _mediator.Send(participantsListQuery, cancellationToken);

            var participant = await _context.Participants.SingleOrDefaultAsync(x => x.UserId == _currentUserService.UserId, cancellationToken);

            var projectParticipantDto = _mapper.Map<ProjectParticipantDto>(participant);

            if (projectParticipantsListVm.Participants.All(x => x.Id != projectParticipantDto.Id))
            {
                throw new BadRequestException($"Participants can delete an issue (regardless of who created it).");
            }

            _context.Issues.Remove(issue);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}