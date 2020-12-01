using FluentValidation;

namespace CleanArchitecture.Application.Issues.Commands.CreateIssue
{
    public class CreateIssueCommandValidator : AbstractValidator<CreateIssueCommand>
    {
        public CreateIssueCommandValidator()
        {
            RuleFor(a => a.Title).NotEmpty().MinimumLength(3).MaximumLength(255);
        }
    }
}