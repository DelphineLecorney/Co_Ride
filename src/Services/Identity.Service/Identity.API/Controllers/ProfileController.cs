using Identity.API.Extensions;
using Identity.Application.Commands.SetUserAvatar;
using Identity.Application.Commands.UpdateUserProfile;
using Identity.Application.Queries.GetUserProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Profile;

namespace Identity.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController(ISender sender) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();

            var result = await sender.Send(new GetUserProfileQuery(userId.Value), cancellationToken);

            return Ok(result);
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetProfileById(Guid userId, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetUserProfileQuery(userId), cancellationToken);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(
            [FromBody] UpdateUserProfileRequest request,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();

            var command = new UpdateUserProfileCommand(
                userId.Value,
                request.FirstName,
                request.LastName,
                request.Bio,
                request.PhoneNumber
            );

            var result = await sender.Send(command, cancellationToken);

            return Ok(result);
        }

        [HttpPut("avatar")]
        public async Task<IActionResult> SetAvatar(
            [FromBody] SetAvatarRequest request,
            CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();

            var command = new SetUserAvatarCommand(userId.Value, request.AvatarUrl);
            var result = await sender.Send(command, cancellationToken);

            return Ok(result);
        }

    }

}
