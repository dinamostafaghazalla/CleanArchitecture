using MediatR;

namespace CleanArchitecture.Application.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommand : IRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}