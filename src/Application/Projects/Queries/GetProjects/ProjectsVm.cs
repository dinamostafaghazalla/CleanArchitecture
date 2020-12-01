using System.Collections.Generic;

namespace CleanArchitecture.Application.Projects.Queries.GetProjects
{
    public class ProjectsVm
    {
        public IList<ProjectDto> Projects { get; set; }

        public int Count { get; set; }
    }
}
