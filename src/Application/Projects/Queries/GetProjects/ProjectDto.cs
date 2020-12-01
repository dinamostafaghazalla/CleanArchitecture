using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Issues;
using CleanArchitecture.Domain.Entities.Participants;
using CleanArchitecture.Domain.Entities.Projects;
using CleanArchitecture.Domain.Enums;
using System.Collections.Generic;

namespace CleanArchitecture.Application.Projects.Queries.GetProjects
{
    public class ProjectDto : IMapFrom<Project>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string Description { get; set; }
        public string Owner { get; private set; }

        public ICollection<ParticipantDto> Participants { get; private set; } = new List<ParticipantDto>();
        public ICollection<IssueDto> Issues { get; private set; } = new List<IssueDto>();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Project, ProjectDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Owner, opt => opt.MapFrom(s => $"{s.Owner.FirstName} {s.Owner.LastName}"))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.Participants, opt => opt.MapFrom(s => s.Participants))
                ;
        }
    }

    public class ParticipantDto : IMapFrom<Participant>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Participant, ParticipantDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                ;
        }
    }

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