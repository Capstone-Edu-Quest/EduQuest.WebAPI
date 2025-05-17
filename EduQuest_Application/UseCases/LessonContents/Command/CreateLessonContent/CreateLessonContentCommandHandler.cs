using AutoMapper;
using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.Materials.Command.CreateLessonContent
{
	public class CreateLessonContentCommandHandler : IRequestHandler<CreateLeaningMaterialCommand, APIResponse>
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly ISystemConfigRepository _systemConfigRepository;
		private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

		public CreateLessonContentCommandHandler(IMaterialRepository materialRepository, 
			IAssignmentRepository assignmentRepository, 
			IQuizRepository quizRepository, 
			ISystemConfigRepository systemConfigRepository, 
			IMapper mapper, IUnitOfWork unitOfWork)
		{
			_materialRepository = materialRepository;
			_assignmentRepository = assignmentRepository;
			_quizRepository = quizRepository;
			_systemConfigRepository = systemConfigRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<APIResponse> Handle(CreateLeaningMaterialCommand request, CancellationToken cancellationToken)
        {
			var apiResponse = new APIResponse();

			// Check if request.Material is null or empty
			if (request.LessonContent == null || !request.LessonContent.Any())
			{
				return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound,
														  MessageCommon.NotFound, "Empty", "name", "Course");
			}

			var listMaterial = new List<Material>();
			object processed = null;
			// Loop over materials and process them
			foreach (var item in request.LessonContent)
			{
				processed = await ProcessMaterialAsync(item, request.UserId);
				if (processed != null)
				{
					switch (processed)
					{
						case Material material:
							listMaterial.Add(material);
							break;

						case Quiz quiz:
							await _quizRepository.Add(quiz);
							break;

						case Assignment assignment:
							await _assignmentRepository.Add(assignment);
							break;
					}
				}
			}
			if(listMaterial.Count > 0)
			{
				await _materialRepository.CreateRangeAsync(listMaterial);
			}

			// Save all changes at once
			if (await _unitOfWork.SaveChangesAsync() > 0)
			{
				return GeneralHelper.CreateSuccessResponse(
					HttpStatusCode.OK,
					MessageCommon.CreateSuccesfully,
					processed,
					"name",
					"Lesson Content"
				);
			}

			return GeneralHelper.CreateErrorResponse(
				HttpStatusCode.BadRequest,
				MessageCommon.CreateFailed,
				"Saving Failed",
				"name",
				"Lesson Content"
			);
		}

		private async Task<Object?> ProcessMaterialAsync(CreateLessonContentRequest item, string userId)
		{
			

			// Fetch system configuration for material type
			var type = Enum.GetName (typeof(TypeOfMaterial), item.Type);
			var value = await _systemConfigRepository.GetByName(type);
			switch ((TypeOfMaterial)item.Type!)
			{
				case TypeOfMaterial.Document:
					return await ProcessDocumentMaterialAsync(item, value, userId);

				case TypeOfMaterial.Video:
					return ProcessVideoMaterialAsync(item, value, userId);

				case TypeOfMaterial.Quiz:
					return await ProcessQuizMaterialAsync(item, value, userId);

				case TypeOfMaterial.Assignment:
					return await ProcessAssignmentMaterialAsync(item, value, userId);

				default:
					break;
			}

			return null;
		}

		private async Task<Material> ProcessDocumentMaterialAsync(CreateLessonContentRequest item, SystemConfig systemConfig, string userId)
		{
			var material = new Material();
			material.Id = Guid.NewGuid().ToString();
			material.Type = GeneralEnums.TypeOfMaterial.Document.ToString();
			material.Title = item.Title;
			material.Description = item.Description;
			material.Duration = (int)systemConfig.Value!;
			material.Content = item.Content;
			material.UserId = userId;
			return material;
		}

		private async Task<Quiz> ProcessQuizMaterialAsync(CreateLessonContentRequest item, SystemConfig systemConfig, string userId)
		{
			var newQuiz = _mapper.Map<Quiz>(item.Quiz);
			newQuiz.Id = Guid.NewGuid().ToString();
			newQuiz.Title = item.Title;
			newQuiz.Description = item.Description;
			newQuiz.UserId = userId;
			await _quizRepository.Add(newQuiz);

			foreach (var question in newQuiz.Questions)
			{
				question.CreatedAt = DateTime.Now.ToUniversalTime();
				question.UpdatedAt = DateTime.Now.ToUniversalTime();
				foreach (var option in question.Options)
				{
					option.CreatedAt = DateTime.Now.ToUniversalTime();
					option.UpdatedAt = DateTime.Now.ToUniversalTime();
				}
			}
			return newQuiz;
		}

		private Material ProcessVideoMaterialAsync(CreateLessonContentRequest item, SystemConfig systemConfig, string userId)
		{
			var material = new Material();
			material.Id = Guid.NewGuid().ToString();
			material.Type = GeneralEnums.TypeOfMaterial.Video.ToString();
			material.Title = item.Title;
			material.Description = item.Description;
			material.Duration = item.Video!.Duration;
			material.UrlMaterial = item.Video.UrlMaterial;
			material.Thumbnail = item.Video.Thumbnail;
			material.UserId = userId;

			/*if (item.Quiz != null)
			{
				await ProcessQuizMaterialAsync(item, systemConfig, material);
			}*/

			return material;
		}

		private async Task<Assignment> ProcessAssignmentMaterialAsync(CreateLessonContentRequest item, SystemConfig systemConfig, string userId)
		{
			var newAssignment = _mapper.Map<Assignment>(item.Assignment);
			newAssignment.Id = Guid.NewGuid().ToString();
			newAssignment.Title = item.Title;
			newAssignment.Description = item.Description;
			newAssignment.UserId = userId;
			await _assignmentRepository.Add(newAssignment);
			return newAssignment;
		}
	}
}
