using CleanArchitecture.Domain.Enums;
using MediatR;

namespace CleanArchitecture.Application.Issues.Commands.UpdateIssueStatus
{
    public class UpdateIssueStatusCommand : IRequest
    {
        public int Id { get; set; }

        public IssueStatus Status { get; set; }
    }
}