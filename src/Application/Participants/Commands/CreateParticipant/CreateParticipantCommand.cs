using MediatR;

namespace CleanArchitecture.Application.Participants.Commands.CreateParticipant
{
    public class CreateParticipantCommand : IRequest<int>
    {
        public string Title { get; set; }

        public string FirstName { get; set; }

        public string Email { get; set; }

        public string LastName { get; set; }

        public int? ProjectId { get; set; }

        public string Password { get; set; }
    }
}