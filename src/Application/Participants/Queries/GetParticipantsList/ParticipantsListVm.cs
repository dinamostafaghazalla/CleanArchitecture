using System.Collections.Generic;

namespace CleanArchitecture.Application.Participants.Queries.GetParticipantsList
{
    public class ParticipantsListVm
    {
        public IList<ParticipantLookupDto> Participants { get; set; }
    }
}
 