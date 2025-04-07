using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using EduQuest_Application.Helper;

namespace EduQuest_Application.UseCases.Materials.Command.DeleteMaterial
{
	public class DeleteMaterialCommandHandler : IRequestHandler<DeleteMaterialCommand, APIResponse>
	{
		private readonly IMaterialRepository _materialRepository;
		private readonly ICourseRepository _courseRepository;
		private readonly IUnitOfWork _unitOfWork;

		public DeleteMaterialCommandHandler(IMaterialRepository materialRepository, ICourseRepository courseRepository, IUnitOfWork unitOfWork)
		{
			_materialRepository = materialRepository;
			_courseRepository = courseRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<APIResponse> Handle(DeleteMaterialCommand request, CancellationToken cancellationToken)
		{
			var isOwner = await _materialRepository.IsOwnerThisMaterial(request.UserId, request.MaterialId);
			if(isOwner == false)
			{
				return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "Not owner", MessageCommon.NotOwner, "name", "material");
			}
			var material = await _materialRepository.GetMaterialWithLesson(request.MaterialId);
			if (material.LessonMaterials.Any()) //Check material is used in any lesson
			{
				var courseIds = material.LessonMaterials.Select(x => x.Lesson.CourseId).ToList();
				var listCourseUseThisMaterial = await _courseRepository.GetByListIds(courseIds);
				bool hasPublicCourse = listCourseUseThisMaterial.Any(course => course.Status == GeneralEnums.StatusCourse.Public.ToString());
				if(!hasPublicCourse)
				{
					await _materialRepository.Delete(request.MaterialId);
				}
			} else
			{
				await _materialRepository.Delete(request.MaterialId);
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
	}
}
