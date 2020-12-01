using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities.Issues;
using CleanArchitecture.Domain.Events.Issues;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Issues.Commands.UpdateIssueStatus
{
    public class UpdateIssueStatusCommandHandler : IRequestHandler<UpdateIssueStatusCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateIssueStatusCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateIssueStatusCommand request, CancellationToken cancellationToken)
        {
            var issue = await _context.Issues.FindAsync(request.Id);

            if (issue == null)
            {
                throw new NotFoundException(nameof(Issue), request.Id);
            }

            issue.SetStatus(request.Status);

            issue.AddDomainEvent(new List<DomainEvent> { new IssueStatusUpdatedEvent(issue) });

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}