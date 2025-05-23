﻿using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Users.Commands.ApproveBecomeInstructor;
using EduQuest_Application.UseCases.Users.Commands.AssignInstructorToExpert;
using EduQuest_Application.UseCases.Users.Commands.BecomeInstructor;
using EduQuest_Application.UseCases.Users.Commands.SwitchRole;
using EduQuest_Application.UseCases.Users.Commands.UpdateStatus;
using EduQuest_Application.UseCases.Users.Commands.UpdateUser;
using EduQuest_Application.UseCases.Users.Commands.UpdateUserRole;
using EduQuest_Application.UseCases.Users.Queries.GetAllUsers;
using EduQuest_Application.UseCases.Users.Queries.GetCurrentUser;
using EduQuest_Application.UseCases.Users.Queries.GetInstructorApplication;
using EduQuest_Application.UseCases.Users.Queries.GetInstructorProfile;
using EduQuest_Application.UseCases.Users.Queries.GetMyInstructorApplicationQuery;
using EduQuest_Application.UseCases.Users.Queries.GetUserByAssignToExpert;
using EduQuest_Application.UseCases.Users.Queries.GetUserByRole;
using EduQuest_Application.UseCases.Users.Queries.GetUserProfile;
using EduQuest_Application.UseCases.Users.Queries.SearchUser;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EduQuest_API.Controllers;

[Route(Constants.Http.API_VERSION + "/user")]
public class UserController : BaseController
{
	private ISender _mediator;
	public UserController(ISender mediator)
	{
		_mediator = mediator;

	}

    [Authorize]
    [HttpPut("status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> UpdateUserStatus([FromBody] UpdateStatusCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }



    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> SearchUser([FromQuery] SearchUserQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }


    [HttpGet("assignToExpert")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> GetUserByAssignToExpert([FromQuery] GetUserByAsignToExpertQuery request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("becomeInstructor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BecomeInstructor([FromForm] BecomeInstructorCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("assignIntructorToExpert")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignInstructorToExpert([FromBody] AssignIntructorToExpert command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("approveInstructor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ApproveInstructor([FromBody] ApproveBecomeInstructorCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet("status")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersQuery query, CancellationToken cancellationToken = default)
	{ 
		var result = await _mediator.Send(query, cancellationToken);
		return Ok(result);
	}

    [Authorize]
    [HttpGet("myInstructorApplication")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> GetMyInstructorApplication(CancellationToken cancellationToken = default)
    {
        string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new GetMyInstructorApplicationQuery(userId), cancellationToken);
        return Ok(result);
    }

    [HttpPost("cancelInstructorApplication")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> CancelInstructorApplication([FromBody] CancelApplyInstructor command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> GetMyAccount(CancellationToken cancellationToken = default)
    {
        string email = User.GetEmailFromToken().ToString();
        var result = await _mediator.Send(new GetCurrentUserQuery(email), cancellationToken);
        return Ok(result);
    }

    [HttpGet("profile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> GetMyProfile([FromQuery] GetUserProfileQuery profile, CancellationToken cancellationToken = default)
    {
        //string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(profile, cancellationToken);
        return Ok(result);
    }


    //[HttpGet("profile")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<ActionResult<APIResponse>> GetMyProfile(CancellationToken cancellationToken = default)
    //{
    //    string userId = User.GetUserIdFromToken().ToString();
    //    var result = await _mediator.Send(new GetUserProfileQuery(userId), cancellationToken);
    //    return Ok(result);
    //}

    [HttpGet("instructor-profile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> GetInstructorProfile([FromQuery] GetInstructorProfileQuery request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return Ok(result);
    }

	[HttpGet("byRoleId")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<APIResponse>> GetUserByRoleId([FromQuery] GetUserByRoleQuery query, CancellationToken cancellationToken = default)
	{
		var result = await _mediator.Send(query, cancellationToken);
		return Ok(result);
	}


	[Authorize]
    [HttpPut("switch")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> SwitchRole([FromBody] SwitchRoleCommand command,CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    //[Authorize]
    [HttpPut("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> UpdateUser([FromBody] UpdateUserCommand command, CancellationToken cancellationToken = default)
    {
        //string userId = User.GetUserIdFromToken().ToString();
        //command.Id = userId;
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    //[Authorize]
    //[HttpGet("home-info")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<ActionResult<APIResponse>> GetUserOverallInfo([FromQuery] GetUserGameInfoQuery info, CancellationToken cancellationToken = default)
    //{
    //    string email = User.GetEmailFromToken().ToString();
    //    var result = await _mediator.Send(info, cancellationToken);
    //    return Ok(result);
    //}
}
