using MediatR;

namespace CleanArchitecture.Application.Projects.Commands.AddProjectParticipant
{
    public class AddProjectParticipantCommand : IRequest<int>
    {
        public string ParticipantEmail { get; set; }
        public int ProjectId { get; set; }
    }
}