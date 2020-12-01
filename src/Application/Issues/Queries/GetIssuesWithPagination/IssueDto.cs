using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Issues;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.Issues.Queries.GetIssuesWithPagination
{
    public class IssueDto : IMapFrom<Issue>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int? ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string Reporter { get; private set; }

        public string Assignee { get; private set; }

        public IssueType Type { get; set; }

        public IssueStatus Status { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Issue, IssueDto>()
                .ForMember(d => d.ProjectId, opt => opt.MapFrom(s => s.Project.Id))
                .ForMember(d => d.ProjectName, opt => opt.MapFrom(s => s.Project != null ? s.Project.Name : string.Empty))
                .ForMember(d => d.Assignee, opt => opt.MapFrom(s => $"{s.Assignee.FirstName} {s.Assignee.LastName}"))
                .ForMember(d => d.Reporter, opt => opt.MapFrom(s => $"{s.Reporter.FirstName} {s.Reporter.LastName}"))
                ;
        }
    }
}