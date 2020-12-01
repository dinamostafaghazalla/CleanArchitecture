using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Projects;

namespace CleanArchitecture.Application.Participants.Queries.GetParticipantDetail
{
    public class ParticipantProjectDto : IMapFrom<Project>
    {
        public string ProjectId { get; set; }

        public string Project { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Project, ParticipantProjectDto>()
                .ForMember(d => d.ProjectId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Project, opts => opts.MapFrom(s => s.Key));
        }
    }
}