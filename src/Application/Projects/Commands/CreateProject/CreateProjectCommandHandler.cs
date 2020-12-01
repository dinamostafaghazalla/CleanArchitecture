using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities.Projects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Projects.Commands.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateProjectCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var exist = await _context.Projects.AnyAsync(x => x.Key == request.Key, cancellationToken);
            if (exist)
            {
                throw new BadRequestException("keys are unique in the system and cannot be changed.");
            }


            var owner = await _context.Participants.SingleOrDefaultAsync(x => x.UserId == _currentUserService.UserId, cancellationToken);

            var project = new Project(owner, request.Name, request.Key);

            await _context.Projects.AddAsync(project, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return project.Id;
        }
    }
}