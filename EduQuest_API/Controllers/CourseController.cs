﻿using EduQuest_Application.DTO.Request.Courses;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Courses.Command.AssignCourseToExpert;
using EduQuest_Application.UseCases.Courses.Command.AttemptAssignment;
using EduQuest_Application.UseCases.Courses.Command.AttemptQuiz;
using EduQuest_Application.UseCases.Courses.Command.CreateCourse;
using EduQuest_Application.UseCases.Courses.Command.ReviewAssignment;
using EduQuest_Application.UseCases.Courses.Command.SubmitCourse;
using EduQuest_Application.UseCases.Courses.Command.UpdateCourse;
using EduQuest_Application.UseCases.Courses.Queries;
using EduQuest_Application.UseCases.Courses.Queries.GetCourseById;
using EduQuest_Application.UseCases.Courses.Queries.SearchCourse;
using EduQuest_Application.UseCases.Courses.Query.GetAssignmentAttempt;
using EduQuest_Application.UseCases.Courses.Query.GetAssignmentAttemptsForIns;
using EduQuest_Application.UseCases.Courses.Query.GetCourseByAssignToUser;
using EduQuest_Application.UseCases.Courses.Query.GetCourseByStatus;
using EduQuest_Application.UseCases.Courses.Query.GetCourseDetailForIntructor;
using EduQuest_Application.UseCases.Courses.Query.GetCourseStatisticForInstructor;
using EduQuest_Application.UseCases.Courses.Query.GetCourseStudying;
using EduQuest_Application.UseCases.Courses.Query.GetLearnerAssignmentAttempts;
using EduQuest_Application.UseCases.Courses.Query.GetLearnerDetailForInstructor;
using EduQuest_Application.UseCases.Courses.Query.GetLearnerOverviewForInstructor;
using EduQuest_Application.UseCases.Courses.Query.GetLessonMaterials;
using EduQuest_Application.UseCases.Courses.Query.GetQuizAttempts;
using EduQuest_Application.UseCases.Expert.Commands.ApproveCourse;
using EduQuest_Application.UseCases.Revenue.Query.GetCourseRevenue;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Models.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace EduQuest_API.Controllers
{
	[Route(Constants.Http.API_VERSION + "/course")]
    public class CourseController : BaseController
    {
        private ISender _mediator;
        public CourseController(ISender mediator)
        {
            _mediator = mediator;

        }

        [HttpPut("submitCourse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SubmitCourse([FromBody] SubmitCourseCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = Constants.PolicyType.Expert)]
        [HttpGet("assign")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAssignedCourse([FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10, CancellationToken cancellationToken = default)
        {
            var userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new GetCourseByAssignToUserQuery(userId, pageNo, eachPage), cancellationToken);
            return Ok(result);
        }

        [HttpPut("approve")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ApprovePendingCourse([FromBody] ApproveCourseCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPut("assign")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ApprovePendingCourse([FromBody] AssignExpertCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpGet("status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCourseByStatus([FromQuery] GetCourseByStatusQuery query, CancellationToken cancellationToken = default)
        {
            string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("searchCourse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchCourse([FromQuery] SearchCourseRequestDto request, [FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10, CancellationToken cancellationToken = default)
        {
            string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new SearchCourseQuery(pageNo, eachPage, userId, request), cancellationToken);
            return Ok(result);
        }

        [HttpGet("courseDetailForInstructor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CourseDetailResponseForIntructor([FromQuery] string courseId, CancellationToken cancellationToken = default)
        {
            string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new GetCourseDetailForIntructorQuery(userId, courseId), cancellationToken);
            return Ok(result);
        }

        [HttpGet("courseStatisticOverview")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCourseStatisticForInstructor(CancellationToken cancellationToken = default)
        {
            string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new GetCourseStatisticForInstructorQuery(userId), cancellationToken);
            return Ok(result);
        }

        //[HttpGet("recommendedCourse")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> GetRecommnededCourse([FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10, CancellationToken cancellationToken = default)
        //{
        //	string userId = User.GetUserIdFromToken().ToString();
        //	var result = await _mediator.Send(new GetRecommendedCourseQuery(userId, pageNo, eachPage), cancellationToken);
        //	return Ok(result);
        //}

        [HttpGet("byCourseId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchCourseById([FromQuery] string courseId, CancellationToken cancellationToken = default)
        {
            string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new GetCourseByIdQuery(userId, courseId), cancellationToken);

            return Ok(result);
        }

        [HttpGet("studying")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCourseStudying(CancellationToken cancellationToken = default)
        {
            string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new GetCourseStudyingQuery(userId), cancellationToken);
            return Ok(result);
        }

        //[Authorize]
        [HttpGet("lesson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMaterialsByLessonId(//[FromQuery] string userId,
            [FromQuery, Required] string lessonId,
            CancellationToken cancellationToken = default)
        {
            string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new GetLessonMaterialsQuery(lessonId, userId), cancellationToken);
            return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
        }

        [Authorize]
        [HttpGet("createdByMe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCourseByUserId([FromQuery, Range(1, int.MaxValue)] int pageNo = 1, int eachPage = 10, CancellationToken cancellationToken = default)
        {
            string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new GetCourseCreatedByMeQuery(userId, pageNo, eachPage), cancellationToken);
            return Ok(result);
        }

		[HttpGet("learnerOverview")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetLearnerStatistic([FromQuery] string courseId, CancellationToken cancellationToken = default)
		{
			var result = await _mediator.Send(new GetLearnerOverviewForInstructorQuery(courseId), cancellationToken);
			return Ok(result);
		}

		[HttpGet("learnerDetail")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetLearnerDetailInCourse([FromQuery] string userId, string courseId, CancellationToken cancellationToken = default)
		{
			var result = await _mediator.Send(new GetLearnerDetailForInstructorQuery(userId, courseId), cancellationToken);
			return Ok(result);
		}

		

		[Authorize]
        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest request, CancellationToken cancellationToken = default)
        {
            string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new CreateCourseCommand(request, userId), cancellationToken);
            return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
        }

        [Authorize]
        [HttpPut("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCourse([FromBody] UpdateCourseRequest request, CancellationToken cancellationToken = default)
        {
            string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new UpdateCourseCommand(userId, request), cancellationToken);
            return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
        }

        [Authorize(Roles = "Learner")]
        [HttpPost("quiz/attemt")]
        public async Task<IActionResult> AttemptQuiz([FromQuery] string lessonId,
            [FromBody] AttemptQuizDto attempt,
            CancellationToken token = default)
        {
            string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new AttemptQuizCommand(userId, lessonId, attempt), token);

            if (result.Errors != null && result.Errors.StatusResponse == HttpStatusCode.NotFound)
            {
                return NotFound(result);
            }
            return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
        }
        [Authorize(Roles = "Learner")]
        [HttpPost("assignment/attemt")]
        public async Task<IActionResult> AttemptAssignment([FromQuery] string lessonId,
            [FromBody] AttemptAssignmentDto attempt,
            CancellationToken token = default)
        {
            string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new AttemptAssignmentCommand(userId, lessonId, attempt), token);
            if (result.Errors != null && result.Errors.StatusResponse == HttpStatusCode.NotFound)
            {
                return NotFound(result);
            }
            return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
        }
        [Authorize]
        [HttpPost("assignment/review")]
        public async Task<IActionResult> AttemptReview([FromBody] GradingAssignmentDto grading,
            CancellationToken token = default)
        {
            string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new ReviewAssignmentCommand(userId, grading), token);
            if (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.NotFound)
            {
                return NotFound(result);
            }
            return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
        }

        [Authorize(Roles = "Learner")]
        [HttpGet("assignment/attempt")]
        public async Task<IActionResult> GetLearnersAssignmentAttempt([FromQuery] string assignmentId,
            [FromQuery] string lessonId,
            CancellationToken token = default)
        {
            string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new GetAssignmentAttemptQuery(userId, assignmentId, lessonId), token);
            if (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.NotFound)
            {
                return NotFound(result);
            }
            return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
        }
        [Authorize(Roles = "Instructor")]
        [HttpGet("assignment/instructor/attempts")]
        public async Task<IActionResult> ViewAssignmentAttempts([FromQuery] string assignmentId,
            [FromQuery] string lessonId,
            CancellationToken token = default)
        {
            var result = await _mediator.Send(new GetLearnerAssignmentAttemptsQuery(assignmentId, lessonId), token);
            return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
        }


        [Authorize(Roles = "Learner")]
        [HttpGet("quiz/attemt")]
        public async Task<IActionResult> GetQuizAttempts([FromQuery] string quizId,
            [FromQuery] string lessonId,
            CancellationToken token = default)
        {
            string userId = User.GetUserIdFromToken().ToString();
            var result = await _mediator.Send(new GetQuizAttemptsQuery(quizId, lessonId, userId), token);
            return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
        }
        [Authorize(Roles = "Instructor")]
        [HttpGet("assignment/instructor/unreviewedAttempts")]
        public async Task<IActionResult> ViewUnreviewedAssignmentAttempts([FromQuery] string courseId,
            CancellationToken token = default)
        {
            var result = await _mediator.Send(new GetAssignmentAttemptsForInsQuery(courseId), token);
            return (result.Errors != null && result.Errors.StatusResponse != HttpStatusCode.OK) ? BadRequest(result) : Ok(result);
        }
    }
}
