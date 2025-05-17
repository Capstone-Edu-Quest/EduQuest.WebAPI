using AutoMapper;
using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.LessonContents.Command.UpdateLessonContent
{
	public class UpdateLessonContentCommandHandler : IRequestHandler<UpdateLessonContentCommand, APIResponse>
	{
		private readonly IMaterialRepository _materialRepository;
		private readonly IAssignmentRepository _assignmentRepository;
		private readonly IQuizRepository _quizRepository;
		private readonly IQuestionRepository _questionRepository;
		private readonly IOptionRepository _answerRepository;
		private readonly ILessonContentRepository _lessonMaterialRepository;
		private readonly ISystemConfigRepository _systemConfigRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public UpdateLessonContentCommandHandler(IMaterialRepository materialRepository, IAssignmentRepository assignmentRepository, IQuizRepository quizRepository, IQuestionRepository questionRepository, IOptionRepository answerRepository, ILessonContentRepository lessonMaterialRepository, ISystemConfigRepository systemConfigRepository, IUnitOfWork unitOfWork, IMapper mapper)
		{
			_materialRepository = materialRepository;
			_assignmentRepository = assignmentRepository;
			_quizRepository = quizRepository;
			_questionRepository = questionRepository;
			_answerRepository = answerRepository;
			_lessonMaterialRepository = lessonMaterialRepository;
			_systemConfigRepository = systemConfigRepository;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<APIResponse> Handle(UpdateLessonContentCommand request, CancellationToken cancellationToken)
		{
			//var isOwner = await _materialRepository.IsOwnerThisMaterial(request.UserId, request.LessonContent.Id);
			//if (isOwner == false)
			//{
			//	return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "Not owner", MessageCommon.NotOwner, "name", "material");
			//}

			//var material = await _materialRepository.GetMataterialQuizAssById(request.Material.Id);
			var type = Enum.GetName(typeof(TypeOfMaterial), request.LessonContent.Type);
			var value = await _systemConfigRepository.GetByName(type);

			//material.Title = request.Material.Title;
			//material.Description = request.Material.Description;
			object processed = null;

			switch (request.LessonContent.Type)
			{
				case (int)TypeOfMaterial.Document:
					processed = await ProcessDocumentMaterialAsync(request.LessonContent, value, request.UserId);
					break;

				case (int)TypeOfMaterial.Quiz:
					processed = await ProcessQuizMaterialAsync(request.LessonContent, value, request.UserId);
					break;

				case (int)TypeOfMaterial.Video:
					processed = ProcessVideoMaterialAsync(request.LessonContent, value, request.UserId);
					break;

				case (int)TypeOfMaterial.Assignment:
					processed = await ProcessAssignmentMaterialAsync(request.LessonContent, value, request.UserId);
					break;

				default:
					break;
			}
			
			var result = await _unitOfWork.SaveChangesAsync() > 0;

			return new APIResponse
			{
				IsError = !result,
				Payload = result ? processed : null,
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

		private async Task<Material> ProcessDocumentMaterialAsync(UpdateLessonContentRequest item, SystemConfig config, string userId)
		{
			var material = await _materialRepository.GetMataterialQuizAssById(item.Id);
			material.Title = item.Title;
			material.Description = item.Description;
			material.Content = item.Content;
			material.Duration = (int)config.Value!;
			material.UpdatedBy = userId;

			await _materialRepository.Update(material);
			return material;
		}

		private async Task<Quiz> ProcessQuizMaterialAsync(UpdateLessonContentRequest item, SystemConfig config, string userId)
		{
			var isUsed = await _lessonMaterialRepository.IsLessonContentUsed(item.Id);
			if (isUsed)
			{
				return null;
			}

			var quiz = await _quizRepository.GetQuizById(item.Id);
			if (quiz != null)
			{
				var questions = quiz.Questions.ToList();
				foreach (var question in questions)
				{
					var listOption = question.Options.ToList();
					_answerRepository.DeleteRange(listOption);
					await _unitOfWork.SaveChangesAsync();
				}

				// Xoá toàn bộ question cũ
				_questionRepository.DeleteRange(questions);

				// Cập nhật lại quiz
				quiz.Title = item.Title;
				quiz.Description = item.Description;
				quiz.UpdatedBy = userId;
				foreach (var question in quiz.Questions)
				{
					question.UpdatedAt = DateTime.Now.ToUniversalTime();
					question.CreatedAt = DateTime.Now.ToUniversalTime();
					foreach (var option in question.Options)
					{
						option.UpdatedAt = DateTime.Now.ToUniversalTime();
						option.CreatedAt = DateTime.Now.ToUniversalTime();
					}
				}
				_mapper.Map(item.Quiz, quiz);
				await _quizRepository.Update(quiz);
				
			}

			//// Tạo mới câu hỏi
			//var newQuestions = _mapper.Map<List<Question>>(item.Quiz.Questions);
			//foreach (var question in newQuestions)
			//{
			//	question.Id = Guid.NewGuid().ToString();
			//	question.QuizId = quiz!.Id;
			//}
			//await _questionRepository.CreateRangeAsync(newQuestions);

			//// Tạo mới câu trả lời
			//var allOptions = new List<Option>();
			//foreach (var questionRequest in item.Quiz.Questions)
			//{
			//	var targetQuestion = newQuestions.First(q => q.QuestionTitle == questionRequest.QuestionTitle);
			//	var options = _mapper.Map<List<Option>>(questionRequest.Options);

			//	foreach (var answer in options)
			//	{
			//		answer.Id = Guid.NewGuid().ToString();
			//		answer.QuestionId = targetQuestion.Id;
			//	}

			//	allOptions.AddRange(options);
			//}
			//await _answerRepository.CreateRangeAsync(allOptions);
			return quiz;
		}


		private async Task<Material> ProcessVideoMaterialAsync(UpdateLessonContentRequest item, SystemConfig config, string userId)
		{
			var material = await _materialRepository.GetMataterialQuizAssById(item.Id);
			material.Title = item.Title;
			material.Description = item.Description;
			material.Duration = item.Video!.Duration;
			material.UrlMaterial = item.Video.UrlMaterial;
			material.Thumbnail = item.Video.Thumbnail;
			material.UpdatedBy = userId;

			/*if (item.Quiz != null)
			{
				await ProcessQuizMaterialAsync(item, config, material);
			}*/
			await _materialRepository.Update(material);
			return material;
		}



		private async Task<Assignment> ProcessAssignmentMaterialAsync(UpdateLessonContentRequest item, SystemConfig config, string userId)
		{
			var isUsed = await _lessonMaterialRepository.IsLessonContentUsed(item.Id);
			if (isUsed)
			{
				return null;
			}
			var assignment = _mapper.Map<Assignment>(item.Assignment);
			assignment.Id = item.Id;
			assignment.Title = item.Title;
			assignment.Description = item.Description;
			assignment.UpdatedBy = userId;
			assignment.UserId = userId;

			await _assignmentRepository.Update(assignment);
			return assignment;
		}

		//private async Task<bool> IsMaterialUsed(string materialId)
		//{
		//	var lessonMaterial = await _lessonMaterialRepository.GetLessonMaterialByMaterialId(materialId);
		//	if (!lessonMaterial.Any())
		//	{
		//		return false;
		//	}
		//	var lessonIds = lessonMaterial.Select(x => x.LessonId).Distinct();
		//	var lessons = await _lessonRepository.GetByIdsAsync(lessonIds);

		//	if (lessons.Any(lesson => lesson.Course.Status == GeneralEnums.StatusCourse.Public.ToString()))
		//	{
		//		return true;
		//	}

		//	return false;
		//}
	}
}
