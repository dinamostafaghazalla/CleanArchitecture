using MediatR;
using System;

namespace CleanArchitecture.Application.Participants.Commands.UpdateParticipant
{
    public class UpdateParticipantCommand : IRequest
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}