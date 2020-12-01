using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Issues;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Projects.Queries.GetProjectIssuesList
{
    public class ProjectIssueDto : IMapFrom<Issue>
    {
        public int Id { get; private set; }
        public string IssueId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Reporter { get; private set; }
        public string Assignee { get; private set; }
        public string Project { get; private set; }
        public IssueStatus Status { get; private set; }
        public IssueType Type { get; private set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Issue, ProjectIssueDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.Title))
                .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Description))
                .ForMember(d => d.Project, opt => opt.MapFrom(s => s.Project.Name))
                .ForMember(d => d.Assignee, opt => opt.MapFrom(s => $"{s.Assignee.FirstName} {s.Assignee.LastName}"))
                .ForMember(d => d.Reporter, opt => opt.MapFrom(s => $"{s.Reporter.FirstName} {s.Reporter.LastName}"))
                ;
        }
    }
}