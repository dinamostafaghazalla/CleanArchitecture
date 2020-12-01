using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Participants;

namespace CleanArchitecture.Application.Participants.Queries.GetParticipantsList
{
    public class ParticipantLookupDto : IMapFrom<Participant>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Participant, ParticipantLookupDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.FirstName + " " + s.LastName))
                .ForMember(d => d.Position, opt => opt.MapFrom(s => s.Title));
        }
    }
}