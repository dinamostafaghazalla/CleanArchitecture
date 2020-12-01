using MediatR;

namespace CleanArchitecture.Application.Projects.Queries.GetProjectDetail
{
    public class GetProjectDetailQuery : IRequest<ProjectDetailVm>
    {
        public int Id { get; set; }
    }
}
