using System.Linq;
using MediatR;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities.Issues;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Application.Projects.Queries.GetProjectParticipantsList;
using CleanArchitecture.Domain.Entities.Participants;
using CleanArchitecture.Domain.Entities.Projects;
using CleanArchitecture.Domain.Exceptions;

namespace CleanArchitecture.Application.Issues.Commands.UpdateIssue
{
    public class UpdateIssueCommandHandler : IRequestHandler<UpdateIssueCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public UpdateIssueCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMediator mediator, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateIssueCommand request, CancellationToken cancellationToken)
        {
            var issue = await _context.Issues.FindAsync(request.Id);

            if (issue == null)
            {
                throw new NotFoundException($"{nameof(Issue)} {request.Id} not found.");
            }

            var participantsListQuery = new GetProjectParticipantsListQuery { ProjectId = issue.ProjectId };

            var projectParticipantsListVm = await _mediator.Send(participantsListQuery, cancellationToken);

            var participant = await _context.Participants.SingleOrDefaultAsync(x => x.UserId == _currentUserService.UserId, cancellationToken);

            var projectParticipantDto = _mapper.Map<ProjectParticipantDto>(participant);

            if (projectParticipantsListVm.Participants.All(x => x.Id != projectParticipantDto.Id))
            {
                throw new DomainException($"Participants can update an issue");
            }

            if (request.ProjectId != null)
            {
                var project = await _context.Projects.SingleOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken);
                if (project == null)
                {
                    throw new NotFoundException($"{nameof(Project)} {request.AssigneeId} not found.");
                }

                issue.SetProject(project);
            }

            if (request.AssigneeId != null)
            {
                var assignee = await _context.Participants.SingleOrDefaultAsync(x => x.Id == request.AssigneeId, cancellationToken);
                if (assignee == null)
                {
                    throw new NotFoundException($"{nameof(Participant)} {request.AssigneeId} not found.");
                }

                issue.SetAssignee(assignee);
            }

            if (!string.IsNullOrEmpty(request.IssueId))
            {
                issue.SetIssueId(request.IssueId);
            }

            issue.SetTitle(request.Title)
                .SetDescription(request.Description)
                .SetStatus(request.Status)
                ;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}