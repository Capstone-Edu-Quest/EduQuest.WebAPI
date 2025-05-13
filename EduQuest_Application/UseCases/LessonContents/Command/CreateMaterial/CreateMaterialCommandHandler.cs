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

namespace EduQuest_Application.UseCases.Materials.Command.CreateMaterial
{
    public class CreateMaterialCommandHandler : IRequestHandler<CreateLeaningMaterialCommand, APIResponse>
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly ISystemConfigRepository _systemConfigRepository;
		private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

		public CreateMaterialCommandHandler(IMaterialRepository materialRepository, ISystemConfigRepository systemConfigRepository, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_materialRepository = materialRepository;
			_systemConfigRepository = systemConfigRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<APIResponse> Handle(CreateLeaningMaterialCommand request, CancellationToken cancellationToken)
        {
			var apiResponse = new APIResponse();

			// Check if request.Material is null or empty
			if (request.Material == null || !request.Material.Any())
			{
				return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound,
														  MessageCommon.NotFound, "Empty", "name", "Course");
			}

			var listMaterial = new List<Material>();

			// Loop over materials and process them
			foreach (var item in request.Material)
			{
				var material = await ProcessMaterialAsync(item, request.UserId);
				if (material != null)
				{
					listMaterial.Add(material);
				}
			}
			await _materialRepository.CreateRangeAsync(listMaterial);

			// Save all changes at once
			if (await _unitOfWork.SaveChangesAsync() > 0)
			{
				return GeneralHelper.CreateSuccessResponse(
					HttpStatusCode.OK,
					MessageCommon.CreateSuccesfully,
					listMaterial,
					"name",
					"Material"
				);
			}

			return GeneralHelper.CreateErrorResponse(
				HttpStatusCode.BadRequest,
				MessageCommon.CreateFailed,
				"Saving Failed",
				"name",
				"Material"
			);
		}

		private async Task<Material?> ProcessMaterialAsync(CreateMaterialRequest item, string userId)
		{
			var material = _mapper.Map<Material>(item);
			material.Id = Guid.NewGuid().ToString();
			material.Type = Enum.GetName(typeof(TypeOfMaterial), item.Type);
			material.UserId = userId;
			material.Version = 1;

			// Fetch system configuration for material type
			var value = await _systemConfigRepository.GetByName(material.Type!);
			switch ((TypeOfMaterial)item.Type!)
			{
				case TypeOfMaterial.Document:
					return await ProcessDocumentMaterialAsync(item, value, material);

				case TypeOfMaterial.Video:
					return ProcessVideoMaterialAsync(item, value, material);

				/*case TypeOfMaterial.Quiz:
					return await ProcessQuizMaterialAsync(item, value, material);

				case TypeOfMaterial.Assignment:
					return await ProcessAssignmentMaterialAsync(item, value, material);*/

				default:
					material.Duration = 0;
					break;
			}

			return material;
		}

		private async Task<Material> ProcessDocumentMaterialAsync(CreateMaterialRequest item, SystemConfig systemConfig, Material material)
		{
			material.Duration = (int)systemConfig.Value!;
			material.Content = item.Content;
			return material;
		}

		/*private async Task<Material> ProcessQuizMaterialAsync(CreateMaterialRequest item, SystemConfig systemConfig, Material material)
		{
			material.Duration = (int)(item.Quiz!.TimeLimit! * systemConfig.Value!);

			// Add new Quiz
			var newQuiz = _mapper.Map<Quiz>(item.Quiz!);
			newQuiz.Id = Guid.NewGuid().ToString();
			material.QuizId = newQuiz.Id;
			await _quizRepository.Add(newQuiz);

			// Add new Questions for the quiz
			var questions = _mapper.Map<List<Question>>(item.Quiz.Questions);
			foreach (var question in questions)
			{
				question.Id = Guid.NewGuid().ToString();
				question.QuizId = newQuiz.Id;
			}
			await _questionRepository.CreateRangeAsync(questions);

			// Add answers for the questions
			var answers = new List<Answer>();
			foreach (var questionRequest in item.Quiz.Questions)
			{
				var question = questions.First(q => q.QuestionTitle == questionRequest.QuestionTitle);
				var answersForQuestion = _mapper.Map<List<Answer>>(questionRequest.Answers);

				foreach (var answer in answersForQuestion)
				{
					answer.Id = Guid.NewGuid().ToString();
					answer.QuestionId = question.Id;
				}

				answers.AddRange(answersForQuestion);
			}

			await _answerRepository.CreateRangeAsync(answers);
			return material;
		}*/

		private Material ProcessVideoMaterialAsync(CreateMaterialRequest item, SystemConfig systemConfig, Material material)
		{
			material.Duration = item.Video!.Duration;
			material.UrlMaterial = item.Video.UrlMaterial;
			material.Thumbnail = item.Video.Thumbnail;

			/*if (item.Quiz != null)
			{
				await ProcessQuizMaterialAsync(item, systemConfig, material);
			}*/

			return material;
		}

		/*private async Task<Material> ProcessAssignmentMaterialAsync(CreateMaterialRequest item, SystemConfig systemConfig, Material material)
		{
			material.Duration = (int)(item.Assignment!.TimeLimit! * systemConfig.Value!);
			var newAssignment = _mapper.Map<Assignment>(item.Assignment);
			newAssignment.Id = Guid.NewGuid().ToString();
			material.AssignmentId = newAssignment.Id;
			await _assignmentRepository.Add(newAssignment);
			return material;
		}*/
	}
}
