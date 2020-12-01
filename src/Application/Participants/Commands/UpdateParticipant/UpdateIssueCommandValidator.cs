using FluentValidation;

namespace CleanArchitecture.Application.Participants.Commands.UpdateParticipant
{
    public class UpdateParticipantCommandValidator : AbstractValidator<UpdateParticipantCommand>
    {
        public UpdateParticipantCommandValidator()
        {
            RuleFor(a => a.FirstName).NotEmpty().MinimumLength(3).MaximumLength(100);
            RuleFor(a => a.LastName).NotEmpty().MinimumLength(3).MaximumLength(100);
            RuleFor(a => a.Title).NotEmpty().MinimumLength(3).MaximumLength(100);
        }
    }
}