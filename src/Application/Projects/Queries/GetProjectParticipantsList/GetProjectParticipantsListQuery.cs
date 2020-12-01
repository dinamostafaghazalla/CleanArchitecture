using MediatR;

namespace CleanArchitecture.Application.Projects.Queries.GetProjectParticipantsList
{
    public class GetProjectParticipantsListQuery : IRequest<ProjectParticipantsListVm>
    {
        public int ProjectId { get; set; }
    }
}
