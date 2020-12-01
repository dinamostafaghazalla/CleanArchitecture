using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Projects;

namespace CleanArchitecture.Application.Projects.Queries.GetProjectDetail
{
    public class ProjectDetailVm : IMapFrom<Project>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }

        public string Owner { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Project, ProjectDetailVm>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.Owner, opt => opt.MapFrom(s => $"{s.Owner.FirstName} {s.Owner.LastName}"))
                ;
        }
    }
}