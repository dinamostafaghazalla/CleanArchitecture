using CleanArchitecture.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Projects.Commands.CreateProject
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateProjectCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(a => a.Name)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(255);

            RuleFor(a => a.Key)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(4)
                .MustAsync(BeUniqueKey)
                .WithMessage("The specified key already exists.");
        }

        public async Task<bool> BeUniqueKey(string key, CancellationToken cancellationToken)
        {
            return await _context.Projects.AllAsync(x => x.Key != key, cancellationToken);
        }
    }
}