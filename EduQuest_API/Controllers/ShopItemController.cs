using EduQuest_Application.UseCases.Shop.Commands;
using EduQuest_Application.UseCases.Shop.Commands.UpdateShopItem;
using EduQuest_Application.UseCases.Shop.Queries.GetItemWithFilterl;
using EduQuest_Application.UseCases.ShopItem.Queries;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest_API.Controllers;

[Route(Constants.Http.API_VERSION + "/shopItem")]
public class ShopItemController : BaseController
{
    private readonly ISender _mediator;

    public ShopItemController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetItemInShop([FromQuery] string userId, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetShopItemsQuery(userId), cancellationToken);
        return Ok(result);
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateNewItems([FromBody] CreateShopItemCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }


    [HttpGet("filter")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetShopItemByFilter([FromQuery] GetShopItemWithFilter request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return Ok(result);
    }


    [HttpPut("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateShopItem([FromBody] UpdateShopItemCommand request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return Ok(result);
    }
}
