using AutoMapper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Stages.Command.CreateStage
{
	public class CreateStageCommandHandler : IRequestHandler<CreateStageCommand, APIResponse>
	{
		
		private readonly IUnitOfWork _unitOfWork;
		private readonly IStageRepository _stageRepository;
		private readonly ICourseRepository _courseRepository;

		public CreateStageCommandHandler(IUnitOfWork unitOfWork, IStageRepository stageRepository, ICourseRepository courseRepository)
		{
			_unitOfWork = unitOfWork;
			_stageRepository = stageRepository;
			_courseRepository = courseRepository;
		}

		public async Task<APIResponse> Handle(CreateStageCommand request, CancellationToken cancellationToken)
		{
			if (request.StageCourse == null || !request.StageCourse.Any())
			{
				return new APIResponse
				{
					IsError = true,
					Errors = new ErrorResponse
					{
						StatusResponse = HttpStatusCode.NotFound,
						StatusCode = (int)HttpStatusCode.NotFound,
						Message = MessageCommon.CreateFailed,
					},
					Message = new MessageResponse
					{
						content = MessageCommon.NoProvided,
						values = new Dictionary<string, string> { { "name", "stage" } }
					}
				};
			}
			var courseExisted = await _courseRepository.GetCourseById(request.CourseId);
			if (courseExisted == null)
			{
				return new APIResponse
				{
					IsError = true,
					Errors = new ErrorResponse
					{
						StatusResponse = HttpStatusCode.NotFound,
						StatusCode = (int)HttpStatusCode.NotFound,
						Message = MessageCommon.NotFound,
					},
					Message = new MessageResponse
					{
						content = MessageCommon.NotFound,
						values = new Dictionary<string, string> { { "name", $"Stage ID {request.CourseId}" } }
					}
				};
			}
			var stages = new List<Lesson>();
			int level = 1;
			if (courseExisted.Stages!.Any() && courseExisted.Stages != null)
			{
				var maxLevel = await _stageRepository.GetMaxLevelInThisCourse(request.CourseId);
				level = (int)++maxLevel;
			}
			

			foreach (var stageRequest in request.StageCourse)
			{
				var stage = new Lesson
				{
					Id = Guid.NewGuid().ToString(),
					CourseId = request.CourseId,
					Name = stageRequest.Name!,
					Description = stageRequest.Description!,
					Level = level++
				};

				stages.Add(stage);
				
			}
			await _stageRepository.CreateRangeAsync(stages); // Add stage to repository
			var result = await _unitOfWork.SaveChangesAsync() > 0;

			return new APIResponse
			{
				IsError = !result,
				Payload = result ? stages : null,
				Errors = result ? null : new ErrorResponse
				{
					StatusResponse = HttpStatusCode.BadRequest,
					StatusCode = (int)HttpStatusCode.BadRequest,
					Message = MessageCommon.CreateFailed,
				},
				Message = new MessageResponse
				{
					content = result ? MessageCommon.CreateSuccesfully : MessageCommon.CreateFailed,
					values = new Dictionary<string, string> { { "name", "course" } }
				}
			};
		} }
}
