using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Participants;

namespace CleanArchitecture.Application.Projects.Queries.GetProjectParticipantsList
{
    public class ProjectParticipantDto : IMapFrom<Participant>
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Participant, ProjectParticipantDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.Title))
                ;
        }
    }
}