using CleanArchitecture.Application.Projects.Commands.AddProjectParticipant;
using CleanArchitecture.Application.Projects.Commands.CreateProject;
using CleanArchitecture.Application.Projects.Commands.DeleteProject;
using CleanArchitecture.Application.Projects.Commands.RemoveProjectParticipant;
using CleanArchitecture.Application.Projects.Commands.UpdateProject;
using CleanArchitecture.Application.Projects.Queries.GetProjectDetail;
using CleanArchitecture.Application.Projects.Queries.GetProjectIssuesList;
using CleanArchitecture.Application.Projects.Queries.GetProjectParticipantsList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CleanArchitecture.Application.Projects.Queries.GetProjects;

namespace CleanArchitecture.WebUI.Controllers
{
    [Authorize]
    public class ProjectsController : ApiControllerBase
    {
        /// <summary>
        /// Get All Projects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ProjectsVm), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ProjectsVm> GetAll()
        {
            return await Mediator.Send(new GetProjectsQuery());
        }

        /// <summary>
        /// Get Project Details
        /// </summary>
        /// <param name="id">Project Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ActionResult<ProjectDetailVm>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ProjectDetailVm>> Get(int id)
        {
            var vm = await Mediator.Send(new GetProjectDetailQuery { Id = id });

            return Ok(vm);
        }

        /// <summary>
        /// Get Project Issues
        /// </summary>
        /// <param name="id">Project Id</param>
        /// <param name="query">filteration</param>
        /// <returns></returns>
        [HttpGet("{id}/issues")]
        [ProducesResponseType(typeof(ProjectDetailVm), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetIssuesList(int id, [FromQuery] GetProjectIssuesListQuery query)
        {
            if (id != query.ProjectId) return BadRequest();

            var vm = await Mediator.Send(query);

            return Ok(vm);
        }

        /// <summary>
        /// Get Project Participants
        /// </summary>
        /// <param name="id">Project Id</param>
        /// <param name="query">filteration</param>
        /// <returns></returns>
        [HttpGet("{id}/participants")]
        [ProducesResponseType(typeof(ProjectDetailVm), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> GetParticipantsList(int id, [FromQuery] GetProjectParticipantsListQuery query)
        {
            if (id != query.ProjectId) return BadRequest();

            var vm = await Mediator.Send(query);

            return Ok(vm);
        }

        /// <summary>
        /// Create new project
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProjectDetailVm), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<int>> Create(CreateProjectCommand command)
        {
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Update Project
        /// </summary>
        /// <param name="id">Project Id</param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProjectDetailVm), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update(int id, UpdateProjectCommand command)
        {
            if (id != command.Id) return BadRequest();

            await Mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Add Project Participant
        /// </summary>
        /// <param name="id">Project Id</param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}/participant")]
        [ProducesResponseType(typeof(ProjectDetailVm), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> AddParticipant(int id, AddProjectParticipantCommand command)
        {
            if (id != command.ProjectId) return BadRequest();

            await Mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Delete Project
        /// </summary>
        /// <param name="id">Project Id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ProjectDetailVm), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteProjectCommand { Id = id });

            return NoContent();
        }


        /// <summary>
        /// Remove Project Participant
        /// </summary>
        /// <param name="id">Project Id</param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("{id}/Participant")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> RemoveParticipant(int id, RemoveProjectParticipantCommand command)
        {
            if (id != command.ProjectId) return BadRequest();

            await Mediator.Send(command);

            return NoContent();
        }
    }
}