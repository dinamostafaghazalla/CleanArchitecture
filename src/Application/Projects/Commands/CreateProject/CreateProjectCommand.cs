using MediatR;

namespace CleanArchitecture.Application.Projects.Commands.CreateProject
{
    public class CreateProjectCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Key { get; set; }
    }
}