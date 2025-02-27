using Application.UseCases.Authenticate.Commands.SignInWithGoogle;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Authenticate.Commands.LogOut;
using EduQuest_Application.UseCases.Authenticate.Commands.RefreshToken;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest_API.Controllers;

[Route(Constants.Http.API_VERSION + "/auth")]
public class AuthenticateController : BaseController
{
    private ISender _mediator;
    public AuthenticateController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("sign-in")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignInGoogle([FromBody] SignInGoogleCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    //[HttpPost("sign-in/test")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> SignInGoogleTest([FromBody] SignInGoogleCommandTest command, CancellationToken cancellationToken)
    //{
    //    var result = await _mediator.Send(command, cancellationToken);
    //    return Ok(result);
    //}

    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("sign-out")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SignOut([FromBody] string accessToken, CancellationToken cancellationToken)
    {
        string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new SignOutCommand { userId = userId, accessToken = accessToken }, cancellationToken);
        return Ok(result);
    }
}
