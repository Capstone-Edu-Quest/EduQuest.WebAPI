using AutoMapper;
using EduQuest_Application.UseCases.Courses.Cpmmands.CreateCourse;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.Courses.Command.CreateCourse
{
	public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;
		private readonly ISystemConfigRepository _systemConfigRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserRepository _userRepository;
		private readonly IStageRepository _stageRepository;
		private readonly ILearningMaterialRepository _learningMaterialRepository;

		public CreateCourseCommandHandler(ICourseRepository courseRepository, ISystemConfigRepository systemConfigRepository, 
			IMapper mapper, IUnitOfWork unitOfWork, 
			IUserRepository userRepository, 
			IStageRepository stageRepository, 
			ILearningMaterialRepository learningMaterialRepository)
		{
			_courseRepository = courseRepository;
			_systemConfigRepository = systemConfigRepository;
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
				var stages = request.CourseRequest.StageCourse.Select(async stagerequest =>
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
					await _stageRepository.Add(stage);
					await _unitOfWork.SaveChangesAsync();
					i++;
					

					if(stagerequest.LearningMaterial != null)
					{
						foreach(var learningMaterial in stagerequest.LearningMaterial)
						{
							var newLM = new LearningMaterial
							{
								StageId = stage.Id,
								Id = Guid.NewGuid().ToString(),
								Type = Enum.GetName(typeof (TypeOfLearningMetarial),learningMaterial.Type!) ?? string.Empty,
								Title = learningMaterial.Title!,
								Description = learningMaterial.Description!,
								UrlMaterial = learningMaterial.UrlMaterial!
							};
							var value = await _systemConfigRepository.GetByName(newLM.Type);
							switch ((TypeOfLearningMetarial)learningMaterial.Type!)
							{
								case TypeOfLearningMetarial.Docs:
									newLM.Duration = (int)value.Value!;
									break;
								case TypeOfLearningMetarial.Video:
									newLM.Duration = learningMaterial.EstimateTime;
									break;
								case TypeOfLearningMetarial.Quiz:
									newLM.Duration = (int)((learningMaterial.EstimateTime!) * value.Value!);
									break;
								default:
									newLM.Duration = 0;
									break;
							} 
							await _learningMaterialRepository.Add(newLM);
							await _unitOfWork.SaveChangesAsync();
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
