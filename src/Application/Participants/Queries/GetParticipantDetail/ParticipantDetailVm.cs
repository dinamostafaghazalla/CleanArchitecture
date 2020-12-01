using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Participants;
using System.Collections.Generic;

namespace CleanArchitecture.Application.Participants.Queries.GetParticipantDetail
{
    public class ParticipantDetailVm : IMapFrom<Participant>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual List<ParticipantProjectDto> Projects { get; set; }

        public virtual List<ParticipantIssueDto> Issues { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Participant, ParticipantDetailVm>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Projects, opts => opts.MapFrom(s => s.Projects))
                .ForMember(d => d.Issues, opts => opts.MapFrom(s => s.AssignedIssues))
                ;
        }
    }
}