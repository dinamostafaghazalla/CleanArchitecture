using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Issues.Commands.CreateIssue;
using CleanArchitecture.Application.Issues.Commands.DeleteIssue;
using CleanArchitecture.Application.Issues.Commands.UpdateIssue;
using CleanArchitecture.Application.Issues.Commands.UpdateIssueStatus;
using CleanArchitecture.Application.Issues.Queries.GetIssuesWithPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CleanArchitecture.WebUI.Controllers
{
    [Authorize]
    public class IssuesController : ApiControllerBase
    {
        /// <summary>
        /// Get Issues With Pagination
        /// </summary>
        /// <param name="query">Get Issues With Pagination Query</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ActionResult<PaginatedList<IssueDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PaginatedList<IssueDto>>> GetIssuesWithPagination([FromQuery] GetIssuesWithPaginationQuery query)
        {
            return await Mediator.Send(query);
        }

        /// <summary>
        /// Create Issue
        /// </summary>
        /// <param name="command">Create Issue Command</param>
        /// <returns>Issue Id</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ActionResult<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<int>> Create(CreateIssueCommand command)
        {
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Update Issue
        /// </summary>
        /// <param name="id">Issue Id</param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update(int id, UpdateIssueCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Update Issue Status
        /// </summary>
        /// <param name="id">Issue Id</param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("status")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateItemDetails(int id, UpdateIssueStatusCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Delete Issue
        /// </summary>
        /// <param name="id">Issue Id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteIssueCommand { Id = id });

            return NoContent();
        }
    }
}