using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.LearningPaths.Queries.GetMyLearningPaths;
using EduQuest_Application.UseCases.LearningPaths.Queries.GetMyPublicLearningPaths;
using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace EduQuest_API.Controllers;
[Route(Constants.Http.API_VERSION + "/LearningPath")]
public class LearningPathController : Controller
{
    
        private ISender _mediator;
        public LearningPathController(ISender mediator)
        {
            _mediator = mediator;

        }

        //[Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetAllUserLearningPath([FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10, CancellationToken cancellationToken = default)
        {
            string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new GetMyLearningPathQuery(userId, pageNo, eachPage), cancellationToken);
            return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
    }

        [HttpGet("detail")]
        public async Task<IActionResult> GetLearningPathDetail([FromQuery] string learningPathId, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> GetMyPublicLearningPath([FromQuery] string UserId, CancellationToken token = default)
        {
            var result = await _mediator.Send(new GetMyPublicLearningPathQuery(UserId), token);
            return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
    }

        [HttpPost]
        public async Task<IActionResult> CreateLearningPath([FromBody] string body, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateLearningPath([FromBody] string body, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

    }
