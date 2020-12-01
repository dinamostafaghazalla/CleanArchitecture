using MediatR;

namespace CleanArchitecture.Application.Issues.Commands.DeleteIssue
{
    public class DeleteIssueCommand : IRequest
    {
        public int Id { get; set; }
    }
}
