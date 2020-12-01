using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Issues;

namespace CleanArchitecture.Application.Issues.Queries.GetIssuesFile
{
    public class IssueRecordDto : IMapFrom<Issue>
    {
        public string Project { get; set; }

        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Issue, IssueRecordDto>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Title))
                .ForMember(d => d.Project, opt => opt.MapFrom(s => s.Project != null ? s.Project.Name : string.Empty));
        }
    }
}
