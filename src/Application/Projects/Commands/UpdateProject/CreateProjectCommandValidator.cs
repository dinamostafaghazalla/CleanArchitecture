using FluentValidation;

namespace CleanArchitecture.Application.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
        {
            RuleFor(a => a.Id);
            RuleFor(a => a.Name).NotEmpty().MinimumLength(3).MaximumLength(255);
        }
    }
}