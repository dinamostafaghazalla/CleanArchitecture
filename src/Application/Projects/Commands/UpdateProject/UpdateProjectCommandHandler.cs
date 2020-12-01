using MediatR;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities.Projects;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Exceptions;

namespace CleanArchitecture.Application.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public UpdateProjectCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.FindAsync(request.Id);

            if (project == null)
            {
                throw new NotFoundException(nameof(Project), request.Id);
            }

            var owner = await _context.Participants.SingleOrDefaultAsync(x => x.UserId == _currentUserService.UserId, cancellationToken);
            if (owner.UserId != _currentUserService.UserId)
            {
                throw new BadRequestException($"Project owners can update any of their projects.");
            }

            project.SetName(request.Name);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}