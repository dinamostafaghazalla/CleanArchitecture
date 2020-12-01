using System.Collections.Generic;

namespace CleanArchitecture.Application.Projects.Queries.GetProjectParticipantsList
{
    public class ProjectParticipantsListVm
    {
        public IList<ProjectParticipantDto> Participants { get; set; }

        public int Count { get; set; }
    }
}
