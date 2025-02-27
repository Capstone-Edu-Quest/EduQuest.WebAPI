using EduQuest_Application.UseCases.Certificates.Commands.CreateCertificate;
using EduQuest_Application.UseCases.Certificates.Queries.GetFilterCertificate;
using EduQuest_Application.UseCases.Mascot.Commands.EquipMacotItem;
using EduQuest_Application.UseCases.Users.Queries.GetAllUsers;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EduQuest_API.Controllers;

[Route(Constants.Http.API_VERSION + "/certificate")]
[ApiController]
public class CertificateController : ControllerBase
{
    private ISender _mediator;

    public CertificateController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("filter")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllCertificates(
            [FromQuery] GetCertificatesQuery query,
            [FromQuery, Range(1, int.MaxValue)] int pageNo = 1,
            [FromQuery, Range(1, int.MaxValue)] int eachPage = 10,
            CancellationToken cancellationToken = default)
    {
        query.Page ??= pageNo;
        query.EachPage ??= eachPage;
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }


    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateNewCertificate([FromBody] CreateCertificateCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

}
