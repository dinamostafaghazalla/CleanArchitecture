using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Issues;

namespace CleanArchitecture.Application.Participants.Queries.GetParticipantDetail
{
    public class ParticipantIssueDto : IMapFrom<Issue>
    {
        public string IssueId { get; set; }

        public string Issue { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Issue, ParticipantIssueDto>()
                .ForMember(d => d.IssueId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Issue, opts => opts.MapFrom(s => s.IssueId));
        }
    }
}