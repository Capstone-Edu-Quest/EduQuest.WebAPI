using AutoMapper;
using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.Materials.Command.UpdateLeaningMaterial
{
	public class UpdateLeaningMaterialCommandHandler : IRequestHandler<UpdateLeaningMaterialCommand, APIResponse>
	{
		private readonly IMaterialRepository _materialRepository;
		private readonly ICourseRepository _courseRepository;
		private readonly ISystemConfigRepository _systemConfigRepository;
		private readonly IAssignmentRepository _assignmentRepository;
		private readonly IQuizRepository _quizRepository;
		private readonly IQuestionRepository _questionRepository;
		private readonly IAnswerRepository _answerRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

	

		public async Task<APIResponse> Handle(UpdateLeaningMaterialCommand request, CancellationToken cancellationToken)
		{
			var isOwner = await _materialRepository.IsOwnerThisMaterial(request.UserId, request.Material.Id);
			if (isOwner == false)
			{
				return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "Not owner", MessageCommon.NotOwner, "name", "material");
			}

			var oldMaterial = await _materialRepository.GetMaterialWithLesson(request.Material.Id);
			var value = await _systemConfigRepository.GetByName(oldMaterial.Type!);  

			//Check hasLearners
			var lessons = oldMaterial.Lessons;
			var listCourse = new List<Course>();
			bool hasLearners = false;
			var courseIds = lessons.Select(l => l.CourseId).Distinct();
			foreach (var courseId in courseIds)
			{
				var course = await _courseRepository.GetCourseLearnerByCourseId(courseId);
				hasLearners = course.CourseLearners != null && course.CourseLearners.Any();
				if (hasLearners) continue;
			}
			Material newMaterial = null;
			if (hasLearners)
			{
				newMaterial.Id = Guid.NewGuid().ToString();
				newMaterial.Title = request.Material.Title;
				newMaterial.Description = request.Material.Description;
				newMaterial.Type = oldMaterial.Type;
				newMaterial.Duration = 0;
				newMaterial.OriginalMaterialId = oldMaterial.Id;
				newMaterial.Version = oldMaterial.Version++;
				await _materialRepository.Add(newMaterial);
			}
			switch (Enum.Parse(typeof(TypeOfMaterial), oldMaterial.Type))
			{
				case TypeOfMaterial.Document:
					if (hasLearners)
					{
						newMaterial = await ProcessDocumentMaterialAsync(request.Material, value, oldMaterial, newMaterial, hasLearners);
					} else
					{
						oldMaterial = await ProcessDocumentMaterialAsync(request.Material, value, oldMaterial, newMaterial, hasLearners);
					}
					
					break;

				//case TypeOfMaterial.Video:
				//	material = await ProcessVideoMaterialAsync(request, value, material, userId);
				//	break;

				//case TypeOfMaterial.Quiz:
				//	material = await ProcessQuizMaterialAsync(request, value, material, userId);
				//	break;

				//case TypeOfMaterial.Assignment:
				//	material = await ProcessAssignmentMaterialAsync(request, value, material);
				//	break;

				default:
					break;
			}
			if (hasLearners)
			{
				await _materialRepository.Update(newMaterial);
			} else
			{
				await _materialRepository.Update(oldMaterial);
			}
			

			var result = await _unitOfWork.SaveChangesAsync() > 0;

			return new APIResponse
			{
				IsError = !result,
				Payload = result ? (hasLearners ? newMaterial : oldMaterial) : null,
				Errors = result ? null : new ErrorResponse
				{
					StatusResponse = HttpStatusCode.BadRequest,
					StatusCode = (int)HttpStatusCode.BadRequest,
					Message = MessageCommon.UpdateFailed,
				},
				Message = new MessageResponse
				{
					content = result ? MessageCommon.UpdateSuccesfully : MessageCommon.UpdateFailed,
					values = new Dictionary<string, string> { { "name", "learning material" } }
				}
			};
		}

		private async Task<Material> ProcessDocumentMaterialAsync(UpdateLearningMaterialRequest item, SystemConfig systemConfig, Material oldMaterial, Material? newMaterial, bool hasLearners)
		{
			if (!hasLearners)
			{
				oldMaterial.Content = item.Content;
				return oldMaterial;
			} else {
				newMaterial.Content = item.Content;
				newMaterial.Duration = (int)systemConfig.Value!;
				return newMaterial;
			}
		}

		//private async Task<Material> ProcessQuizMaterialAsync(UpdateLearningMaterialRequest item, SystemConfig systemConfig, Material oldMaterial, Material? newMaterial, bool hasLearners, string userId)
		//{
		//	if (!hasLearners)
		//	{
		//		oldMaterial.Duration = (int)(item.QuizRequest!.TimeLimit! * (systemConfig?.Value ?? 1));

		//		var quiz = await _quizRepository.GetQuizById(oldMaterial.QuizId);
		//		if (quiz != null)
		//		{
		//			quiz = _mapper.Map<Quiz>(item.QuizRequest);
					
		//			await _quizRepository.Update(quiz);
		//		}
		//	}
		//}

	}
}
