using AutoMapper;
using EduQuest_Application.UseCases.Courses.Cpmmands.CreateCourse;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using System.Runtime.InteropServices;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Courses.Command.CreateCourse
{
	public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserRepository _userRepository;
		private readonly IStageRepository _stageRepository;
		private readonly ILearningMaterialRepository _learningMaterialRepository;

		public CreateCourseCommandHandler(ICourseRepository courseRepository, IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository, IStageRepository stageRepository, ILearningMaterialRepository learningMaterialRepository)
		{
			_courseRepository = courseRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_userRepository = userRepository;
			_stageRepository = stageRepository;
			_learningMaterialRepository = learningMaterialRepository;
		}

		public async Task<APIResponse> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
		{
			var course = _mapper.Map<Course>(request.CourseRequest);
			
			var user = await _userRepository.GetById(request.UserId);
			
			course.CreatedBy = user!.Id;
			course.Id = Guid.NewGuid().ToString();
			course.LastUpdated = DateTime.UtcNow.ToLocalTime();
			await _courseRepository.Add(course);
			await _unitOfWork.SaveChangesAsync(); //Save course
			if(request.CourseRequest.StageCourse != null && request.CourseRequest.StageCourse.Any())
			{
				var stages = request.CourseRequest.StageCourse.Select(stagerequest =>
				{
					int i = 1;
					var stage = new Stage
					{
						Id = Guid.NewGuid().ToString(),
						CourseId = course.Id,
						Name = stagerequest.Name!,
						Description = stagerequest.Description!,
						Level = 1
					};
					_stageRepository.Add(stage);
					_unitOfWork.SaveChangesAsync();
					i++;
					

					if(stagerequest.LearningMaterial != null)
					{
						foreach(var learningMaterial in stagerequest.LearningMaterial)
						{
							var newLM = new LearningMaterial
							{
								StageId = stage.Id,
								Id = Guid.NewGuid().ToString(),
								Type = learningMaterial.Type!,
								Title = learningMaterial.Title!,
								Description = learningMaterial.Description!,
								UrlMaterial = learningMaterial.UrlMaterial!
							};
							_learningMaterialRepository.Add(newLM);
							_unitOfWork.SaveChangesAsync();
						}
					}
					return stage;
				}).ToList();
			}
			return new APIResponse
			{
				IsError = false,
				Payload = course,
				Errors = null,
			//} : new APIResponse
			//{
			//	IsError = true,
			//	Payload = null,
			//	Errors = new ErrorResponse
			//	{
			//		StatusResponse = HttpStatusCode.BadRequest, 
			//		StatusCode = (int)HttpStatusCode.BadRequest,
			//		Message = MessageCommon.SavingFailed,
			//	}
			};

		}
	}
}
