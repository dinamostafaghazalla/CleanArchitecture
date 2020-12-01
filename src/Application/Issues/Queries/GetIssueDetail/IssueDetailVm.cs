using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Issues;

namespace CleanArchitecture.Application.Issues.Queries.GetIssueDetail
{
    public class IssueDetailVm : IMapFrom<Issue>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string SupplierCompanyName { get; set; }

        public int? ProjectId { get; set; }

        public string ProjectName { get; set; }

        public bool EditEnabled { get; set; }

        public bool DeleteEnabled { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Issue, IssueDetailVm>()
                .ForMember(d => d.EditEnabled, opt => opt.Ignore())
                .ForMember(d => d.DeleteEnabled, opt => opt.Ignore())
                .ForMember(d => d.ProjectName, opt => opt.MapFrom(s => s.Project != null ? s.Project.Name : string.Empty));
        }
    }
}