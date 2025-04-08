using EduQuest_Application.DTO.Request.ShopItem;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Mascots.Commands.EquipMacotItem;
using EduQuest_Application.UseCases.Mascots.Commands.PurchaseMascot;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest_API.Controllers;

[Route(Constants.Http.API_VERSION + "/mascot")]
public class MascotController : BaseController
{
    private readonly ISender _mediator;

    public MascotController(ISender mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost("equip")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EquipItem([FromBody] MascotRequestDto request, CancellationToken cancellationToken = default)
    {
        string userId = User.GetUserIdFromToken().ToString();
        var result = await _mediator.Send(new EquipMascotItemCommand
        {
            UserId = userId,
            ItemIds = request.items
        }, cancellationToken);

        return Ok(result);
    }


    //[Authorize]
    [HttpPost("purchase")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PurchaseItem([FromBody] PurchaseMascotItemCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
