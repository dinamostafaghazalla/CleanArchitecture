using FluentValidation;

namespace CleanArchitecture.Application.Projects.Commands.AddProjectParticipant
{
    public class AddProjectParticipantCommandValidator : AbstractValidator<AddProjectParticipantCommand>
    {
        public AddProjectParticipantCommandValidator()
        {
            RuleFor(a => a.ParticipantEmail).NotEmpty().MinimumLength(3).EmailAddress();
        }
    }
}