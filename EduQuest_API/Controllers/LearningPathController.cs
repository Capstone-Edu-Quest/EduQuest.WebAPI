using EduQuest_Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EduQuest_API.Controllers;
[Route(Constants.Http.API_VERSION + "/LearningPath")]
public class LearningPathController : Controller
{
    
        private ISender _mediator;
        public LearningPathController(ISender mediator)
        {
            _mediator = mediator;

        }

        [HttpGet("me")]
        public async Task<IActionResult> GetAllUserLearningPath([FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10, CancellationToken cancellationToken = default)
        {
            /*var result = await _mediator.Send(new GetAllUsersQuery(pageNo, eachPage), cancellationToken);
            return Ok(result);*/
            throw new NotImplementedException();
        }

        [HttpGet("detail")]
        public async Task<IActionResult> GetLearningPathDetail([FromQuery] string learningPathId, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> GetMyPublicLearningPath([FromQuery] string learningPathId, CancellationToken token = default)
        {
            throw new NotImplementedException();
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
