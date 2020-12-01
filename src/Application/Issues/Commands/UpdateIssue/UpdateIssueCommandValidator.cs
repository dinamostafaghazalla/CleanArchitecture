using FluentValidation;

namespace CleanArchitecture.Application.Issues.Commands.UpdateIssue
{
    public class UpdateIssueCommandValidator : AbstractValidator<UpdateIssueCommand>
    {
        public UpdateIssueCommandValidator()
        {
            RuleFor(a => a.Title).NotEmpty().MinimumLength(3).MaximumLength(255);
        }
    }
}