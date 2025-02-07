using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduQuest_API.Controllers;

[Route(Constants.Http.API_VERSION + "/Feedback")]
[ApiController]
public class FeedbackController : ControllerBase
{
    private ISender _mediator;
    public FeedbackController(ISender mediator)
    {
        _mediator = mediator;
    }
}
