using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Projects.Queries.GetProjectParticipantsList;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities.Issues;
using CleanArchitecture.Domain.Entities.Projects;
using CleanArchitecture.Domain.Events.Issues;
using CleanArchitecture.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Issues.Commands.CreateIssue
{
    public class CreateIssueCommandHandler : IRequestHandler<CreateIssueCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CreateIssueCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMediator mediator, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateIssueCommand request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.Include(x => x.Issues)
                .SingleOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken);

            if (project == null)
            {
                throw new NotFoundException($"{nameof(Project)} {request.ProjectId} not found.");
            }

            var participantsListQuery = new GetProjectParticipantsListQuery { ProjectId = request.ProjectId };
            var projectParticipantsListVm = await _mediator.Send(participantsListQuery, cancellationToken);

            var reporter = await _context.Participants.SingleOrDefaultAsync(x => x.UserId == _currentUserService.UserId, cancellationToken);

            var projectParticipantDto = _mapper.Map<ProjectParticipantDto>(reporter);

            if (projectParticipantsListVm.Participants.All(x => x.Id != projectParticipantDto.Id))
            {
                throw new DomainException($"Participants can create an issue of any type.");
            }

            var issue = new Issue(project, reporter, request.IssueType, request.Title, request.Status);
            if (request.AssigneeId.HasValue)
            {
                var assignee = await _context.Participants.SingleOrDefaultAsync(x => x.Id == request.AssigneeId, cancellationToken);
                issue.SetAssignee(assignee);
            }

            issue.SetDescription(request.Description);

            issue.AddDomainEvent(new List<DomainEvent> { new IssueCreatedEvent(issue) });

            project.AddIssue(issue);

            await _context.Issues.AddAsync(issue, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return issue.Id;
        }
    }
}