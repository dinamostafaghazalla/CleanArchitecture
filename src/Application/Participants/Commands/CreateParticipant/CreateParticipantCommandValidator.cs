using FluentValidation;

namespace CleanArchitecture.Application.Participants.Commands.CreateParticipant
{
    public class CreateParticipantCommandValidator : AbstractValidator<CreateParticipantCommand>
    {
        public CreateParticipantCommandValidator()
        {
            RuleFor(a => a.FirstName).NotEmpty().MinimumLength(3).MaximumLength(100);
            RuleFor(a => a.LastName).NotEmpty().MinimumLength(3).MaximumLength(100);
            RuleFor(a => a.Title).NotEmpty().MinimumLength(3).MaximumLength(100);
            RuleFor(a => a.Password).NotEmpty().MinimumLength(8).MaximumLength(255);
            RuleFor(a => a.Email).EmailAddress().MinimumLength(3).MaximumLength(255);
        }
    }
}