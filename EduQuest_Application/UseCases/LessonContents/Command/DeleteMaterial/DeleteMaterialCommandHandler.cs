using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using EduQuest_Application.Helper;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.Materials.Command.DeleteMaterial
{
	public class DeleteMaterialCommandHandler : IRequestHandler<DeleteLessonContentCommand, APIResponse>
	{
		private readonly IMaterialRepository _materialRepository;
		private readonly IQuizRepository _quizRepository;
		private readonly IAssignmentRepository _assignmentRepository;
		private readonly ILessonContentRepository _lessonContentRepository;
		private readonly ICourseRepository _courseRepository;
		private readonly IUnitOfWork _unitOfWork;

		public DeleteMaterialCommandHandler(IMaterialRepository materialRepository, IQuizRepository quizRepository, IAssignmentRepository assignmentRepository, ILessonContentRepository lessonContentRepository, ICourseRepository courseRepository, IUnitOfWork unitOfWork)
		{
			_materialRepository = materialRepository;
			_quizRepository = quizRepository;
			_assignmentRepository = assignmentRepository;
			_lessonContentRepository = lessonContentRepository;
			_courseRepository = courseRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<APIResponse> Handle(DeleteLessonContentCommand request, CancellationToken cancellationToken)
		{
			var isOwner = await _materialRepository.IsOwnerThisMaterial(request.UserId, request.MaterialId);
			if(isOwner == false)
			{
				return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "Not owner", MessageCommon.NotOwner, "name", "material");
			}


			var lessonContent = await _lessonContentRepository.GetLessonContentWithLesson(request.MaterialId);
			if (lessonContent.Any() || lessonContent.Count > 0) //Check leson content is used in any lesson
			{
				var courseIds = lessonContent.Select(x => x.Lesson.CourseId).ToList();
				var listCourseUseThisMaterial = await _courseRepository.GetByListIds(courseIds);
				bool hasPublicCourse = listCourseUseThisMaterial.Any(course => course.Status == GeneralEnums.StatusCourse.Public.ToString());
				if(!hasPublicCourse)
				{
					await DeleteByLessonContentId(request.UserId, request.MaterialId);
				}
			} else
			{
				await DeleteByLessonContentId(request.UserId, request.MaterialId);
			}

			var result = await _unitOfWork.SaveChangesAsync() > 0;
			return new APIResponse
			{
				IsError = !result,
				Payload = result ? result : null,
				Errors = result ? null : new ErrorResponse
				{
					StatusResponse = HttpStatusCode.BadRequest,
					StatusCode = (int)HttpStatusCode.BadRequest,
					Message = MessageCommon.SavingFailed,
				},
				Message = new MessageResponse
				{
					content = result ? MessageCommon.DeleteSuccessfully : MessageCommon.DeleteFailed,
					values = new Dictionary<string, string> { { "name", $"material with id {request.MaterialId}" } }
				}
			};

		}

		public async Task DeleteByLessonContentId(string userId, string lessonContentId)
		{
			var listMaterial = await _materialRepository.GetByUserId(userId, null);
			var listAssignment = await _assignmentRepository.GetByUserId(userId, null);
			var listQuiz = await _quizRepository.GetByUserId(userId, null);

			if (listMaterial.Any(m => m.Id == lessonContentId))
			{
				var matchedMaterial = listMaterial.First(m => m.Id == lessonContentId);
				await _materialRepository.Delete(matchedMaterial.Id);
			}

			if (listAssignment.Any(a => a.Id == lessonContentId))
			{
				var matchedAssignment = listAssignment.First(m => m.Id == lessonContentId);
				await _assignmentRepository.Delete(matchedAssignment.Id);
			}

			if (listQuiz.Any(q => q.Id == lessonContentId))
			{
				var matchedQuiz = listQuiz.First(m => m.Id == lessonContentId);
				await _quizRepository.Delete(matchedQuiz.Id);
			}

			
		}
	}
}
