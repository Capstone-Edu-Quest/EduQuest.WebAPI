using EduQuest_Application.UseCases.Certificates.Commands.CreateCertificate;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> GetAllCertificates([FromQuery] GetCertificatesQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }



    //[HttpPost()]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> CreateNewCertificate([FromBody] CreateCertificateCommand command, CancellationToken cancellationToken = default)
    //{
    //    var result = await _mediator.Send(command, cancellationToken);
    //    return Ok(result);
    //}

}
