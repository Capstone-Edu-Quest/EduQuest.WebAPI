using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.UserMetas.Commands.UpdateUserProgress
{
	public class UpdateUserProgressCommandHandler : IRequestHandler<UpdateUserProgressCommand, APIResponse>
	{
		private readonly IUserMetaRepository _userMetaRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICourseRepository _courseRepository;
		private readonly IMaterialRepository _materialRepository;
		private readonly ISystemConfigRepository _systemConfigRepository;

		public UpdateUserProgressCommandHandler(IUserMetaRepository userMetaRepository, IUnitOfWork unitOfWork, ICourseRepository courseRepository, IMaterialRepository materialRepository, ISystemConfigRepository systemConfigRepository)
		{
			_userMetaRepository = userMetaRepository;
			_unitOfWork = unitOfWork;
			_courseRepository = courseRepository;
			_materialRepository = materialRepository;
			_systemConfigRepository = systemConfigRepository;
		}

		public async Task<APIResponse> Handle(UpdateUserProgressCommand request, CancellationToken cancellationToken)
		{
			var userMeta = await _userMetaRepository.GetByUserId(request.UserId);
			var material = await _materialRepository.GetMataterialQuizAssById(request.Info.MaterialId);
			var course = await _courseRepository.GetCourseLearnerByCourseId(request.Info.CourseId);
			var courseLearner = course.CourseLearners.FirstOrDefault(x => x.UserId == request.UserId);

			Enum.TryParse(material.Type, out GeneralEnums.TypeOfMaterial typeEnum);
			var systemConfig = new SystemConfig();
			switch (typeEnum)
			{
				case GeneralEnums.TypeOfMaterial.Video:
					courseLearner.TotalTime += request.Info.Time;
					userMeta.TotalStudyTime += request.Info.Time;
					break;
				case GeneralEnums.TypeOfMaterial.Document:
					systemConfig = await _systemConfigRepository.GetByName(TypeOfMaterial.Document.ToString());
					courseLearner.TotalTime += (int)systemConfig.Value;
					userMeta.TotalStudyTime += (int)systemConfig.Value;
					break;
				case GeneralEnums.TypeOfMaterial.Assignment:
					systemConfig = await _systemConfigRepository.GetByName(TypeOfMaterial.Assignment.ToString());
					courseLearner.TotalTime += (int)systemConfig.Value * material.Assignment.TimeLimit;
					userMeta.TotalStudyTime += (int)systemConfig.Value * material.Assignment.TimeLimit;
					break;
				case GeneralEnums.TypeOfMaterial.Quiz:
					systemConfig = await _systemConfigRepository.GetByName(TypeOfMaterial.Quiz.ToString());
					courseLearner.TotalTime += (int)systemConfig.Value * material.Quiz.TimeLimit;
					userMeta.TotalStudyTime += (int)systemConfig.Value * material.Quiz.TimeLimit;
					break;
				default:
					break;
			}

			courseLearner.ProgressPercentage = ((decimal)courseLearner.TotalTime / course.CourseStatistic.TotalTime) * 100;
			await _courseRepository.Update(course);
			await _userMetaRepository.Update(userMeta);
			var result = await _unitOfWork.SaveChangesAsync() > 0;
			return new APIResponse
			{
				IsError = !result,
				Payload = result ? courseLearner : null,
				Errors = result ? null : new ErrorResponse
				{
					StatusResponse = HttpStatusCode.BadRequest,
					StatusCode = (int)HttpStatusCode.BadRequest,
					Message = MessageCommon.UpdateFailed,
				},
				Message = new MessageResponse
				{
					content = result ? MessageCommon.UpdateSuccesfully : MessageCommon.UpdateFailed,
					values = new Dictionary<string, string> { { "name", "user progess" } }
				}
			};

		}
	}
}
