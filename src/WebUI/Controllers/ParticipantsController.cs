using CleanArchitecture.Application.Participants.Commands.CreateParticipant;
using CleanArchitecture.Application.Participants.Commands.DeleteParticipant;
using CleanArchitecture.Application.Participants.Commands.UpdateParticipant;
using CleanArchitecture.Application.Participants.Queries.GetParticipantDetail;
using CleanArchitecture.Application.Participants.Queries.GetParticipantsList;
using CleanArchitecture.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.WebUtilities;


namespace CleanArchitecture.WebUI.Controllers
{
    [Authorize]
    public class ParticipantsController : ApiControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ParticipantsController> _logger;
        private readonly IEmailSender _emailSender;

        public ParticipantsController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<ParticipantsController> logger, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Get all Participants
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IList<ParticipantLookupDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IList<ParticipantLookupDto>>> GetAll()
        {
            return Ok(await Mediator.Send(new GetParticipantsListQuery()));
        }

        /// <summary>
        /// Get Participant details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ParticipantDetailVm), StatusCodes.Status200OK)]
        public async Task<ActionResult<ParticipantDetailVm>> Get(int id)
        {
            return Ok(await Mediator.Send(new GetParticipantDetailQuery { Id = id }));
        }

        /// <summary>
        /// Create Participant
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [AllowAnonymous]
        public async Task<IActionResult> Create(CreateParticipantCommand command)
        {
            var user = new ApplicationUser { UserName = command.Email, Email = command.Email };
            var result = await _userManager.CreateAsync(user, command.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            _logger.LogInformation("User created a new account with password.");

            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callbackUrl = Url.Page("/Account/ConfirmEmail", pageHandler: null, values: new { area = "Identity", userId = user.Id, code }, protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(command.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return RedirectToPage("RegisterConfirmation", new { email = command.Email });
            }

            await _signInManager.SignInAsync(user, true);

            var id = await Mediator.Send(command);

            return Created("", id);
        }

        /// <summary>
        /// Update Participant
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update(int id, UpdateParticipantCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Delete Participant
        /// </summary>
        /// <param name="id">Participant Id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteParticipantCommand { Id = id });

            return NoContent();
        }
    }
}