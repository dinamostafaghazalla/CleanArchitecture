using System.Collections.Generic;

namespace CleanArchitecture.Application.Projects.Queries.GetProjectIssuesList
{
    public class ProjectIssuesListVm
    {
        public IList<ProjectIssueDto> Issues { get; set; }

        public int Count { get; set; }
    }
}
