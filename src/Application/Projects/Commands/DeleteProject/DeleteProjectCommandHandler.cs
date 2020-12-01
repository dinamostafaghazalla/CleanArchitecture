using MediatR;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities.Projects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DeleteProjectCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects
                .FindAsync(request.Id);
            if (project == null)
            {
                throw new NotFoundException(nameof(Project), request.Id);
            }

            var owner = await _context.Participants.FindAsync(project.OwnerId);
            if (owner == null)
            {
                throw new NotFoundException(nameof(owner), project.OwnerId);
            }

            if (owner.UserId != _currentUserService.UserId)
            {
                throw new BadRequestException($"Project owners can delete any of their projects.");
            }

            _context.Projects.Remove(project);

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}