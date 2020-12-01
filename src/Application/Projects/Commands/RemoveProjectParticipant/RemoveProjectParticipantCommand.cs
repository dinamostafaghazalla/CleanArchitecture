using MediatR;

namespace CleanArchitecture.Application.Projects.Commands.RemoveProjectParticipant
{
    public class RemoveProjectParticipantCommand : IRequest
    {
        public string ParticipantEmail { get; set; }
        public int ProjectId { get; set; }
    }
}