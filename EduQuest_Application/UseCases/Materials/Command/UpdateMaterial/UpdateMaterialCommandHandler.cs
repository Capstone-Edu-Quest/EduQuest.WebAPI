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

namespace EduQuest_Application.UseCases.Materials.Command.UpdateMaterial
{
	public class UpdateMaterialCommandHandler : IRequestHandler<UpdateMaterialCommand, APIResponse>
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

		public UpdateMaterialCommandHandler(IMaterialRepository materialRepository, ICourseRepository courseRepository, ISystemConfigRepository systemConfigRepository, IAssignmentRepository assignmentRepository, IQuizRepository quizRepository, IQuestionRepository questionRepository, IAnswerRepository answerRepository, IUnitOfWork unitOfWork, IMapper mapper)
		{
			_materialRepository = materialRepository;
			_courseRepository = courseRepository;
			_systemConfigRepository = systemConfigRepository;
			_assignmentRepository = assignmentRepository;
			_quizRepository = quizRepository;
			_questionRepository = questionRepository;
			_answerRepository = answerRepository;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<APIResponse> Handle(UpdateMaterialCommand request, CancellationToken cancellationToken)
		{
			var isOwner = await _materialRepository.IsOwnerThisMaterial(request.UserId, request.Material.Id);
			if (isOwner == false)
			{
				return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, "Not owner", MessageCommon.NotOwner, "name", "material");
			}

			var oldMaterial = await _materialRepository.GetMataterialQuizAssById(request.Material.Id);
			var value = await _systemConfigRepository.GetByName(oldMaterial.Type!);  

			//Check hasLearners
			var lessons = oldMaterial.LessonMaterials.Select(x => x.Lesson);
			var listCourse = new List<Course>();
			bool hasLearners = false;
			var courseIds = lessons.Select(l => l.CourseId).Distinct();
			foreach (var courseId in courseIds)
			{
				var course = await _courseRepository.GetCourseLearnerByCourseId(courseId);
				hasLearners = course.CourseLearners != null && course.CourseLearners.Any();
				if (hasLearners) continue;
			}
			Material newMaterial = new Material();
			if (hasLearners)
			{
				newMaterial.Id = Guid.NewGuid().ToString();
				newMaterial.Title = request.Material.Title;
				newMaterial.Description = request.Material.Description;
				newMaterial.Type = oldMaterial.Type;
				newMaterial.Duration = 0;
				newMaterial.UserId = request.UserId;
				newMaterial.OriginalMaterialId = oldMaterial.Id;
				newMaterial.Version = oldMaterial.Version + 1;
				await _materialRepository.Add(newMaterial);
			} else
			{
				oldMaterial.Title = request.Material.Title;
				oldMaterial.Description = request.Material.Description;
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

				case TypeOfMaterial.Quiz:
					if (hasLearners)
					{
						newMaterial = await ProcessQuizMaterialAsync(request.Material, value, oldMaterial, newMaterial, hasLearners);
					}
					else
					{
						oldMaterial = await ProcessQuizMaterialAsync(request.Material, value, oldMaterial, newMaterial, hasLearners);
					}
					break;

				case TypeOfMaterial.Video:
					if (hasLearners)
					{
						newMaterial = await ProcessVideoMaterialAsync(request.Material, value, oldMaterial, newMaterial, hasLearners);
					}
					else
					{
						oldMaterial = await ProcessVideoMaterialAsync(request.Material, value, oldMaterial, newMaterial, hasLearners);
					}
					break;

				case TypeOfMaterial.Assignment:
					if (hasLearners)
					{
						newMaterial = await ProcessAssignmentMaterialAsync(request.Material, value, oldMaterial, newMaterial, hasLearners);
					}
					else
					{
						oldMaterial = await ProcessAssignmentMaterialAsync(request.Material, value, oldMaterial, newMaterial, hasLearners);
					}
					break;

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

		private async Task<Material> ProcessQuizMaterialAsync(UpdateLearningMaterialRequest item, SystemConfig systemConfig, Material oldMaterial, Material? newMaterial, bool hasLearners)
		{
			Quiz quiz = new Quiz(), newQuiz = new Quiz();
			if (!hasLearners)
			{
				oldMaterial.Duration = (int)(item.Quiz!.TimeLimit! * (systemConfig?.Value ?? 1));

				quiz = await _quizRepository.GetQuizById(oldMaterial.QuizId);
				if (quiz != null)
				{
					quiz = _mapper.Map<Quiz>(item.Quiz);
				}
				await _quizRepository.Update(quiz);

				//Delete answer on this course
				foreach(var question in quiz.Questions)
				{
					var listAnswerForThisQuestion = await _answerRepository.GetListAnswerByQuestionId(question.Id);
					var listId = listAnswerForThisQuestion.Select(x => x.Id);
					await _answerRepository.Delete(listId);
				}
				await _unitOfWork.SaveChangesAsync();
				//Delete question on this course
				quiz.Questions.Clear();
			} else
			{
				newQuiz = _mapper.Map<Quiz>(item.Quiz!);
				newQuiz.Id = Guid.NewGuid().ToString();
				newMaterial.QuizId = newQuiz.Id;
				await _quizRepository.Add(newQuiz);
			}

			// Add new Questions for the quiz
			var questions = _mapper.Map<List<Question>>(item.Quiz.Questions);
			foreach (var question in questions)
			{
				question.Id = Guid.NewGuid().ToString();
				if (hasLearners)
				{
					question.QuizId = newQuiz.Id;
				} else
				{
					question.QuizId = quiz.Id;
				}
				
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
			await _unitOfWork.SaveChangesAsync();
			return hasLearners ? newMaterial : oldMaterial;
		}

		private async Task<Material> ProcessVideoMaterialAsync(UpdateLearningMaterialRequest item, SystemConfig systemConfig, Material oldMaterial, Material? newMaterial, bool hasLearners)
		{
			if (!hasLearners)
			{
				oldMaterial.Duration = item.Video!.Duration;
				oldMaterial.UrlMaterial = item.Video.UrlMaterial;
				oldMaterial.Thumbnail = item.Video.Thumbnail;
			} else
			{
				newMaterial.Duration = item.Video!.Duration;
				newMaterial.UrlMaterial = item.Video.UrlMaterial;
				newMaterial.Thumbnail = item.Video.Thumbnail;
			}

			if (item.Quiz != null)
			{
				await ProcessQuizMaterialAsync(item, systemConfig, oldMaterial, newMaterial, hasLearners);
			}
			return hasLearners ? newMaterial : oldMaterial;
		}

		private async Task<Material> ProcessAssignmentMaterialAsync(UpdateLearningMaterialRequest item, SystemConfig systemConfig, Material oldMaterial, Material? newMaterial, bool hasLearners)
		{
			if (!hasLearners)
			{
				var oldAssignment = oldMaterial.Assignment;
				oldAssignment = _mapper.Map<Assignment>(item.Assignment);
				await _assignmentRepository.Update(oldAssignment);
			}
			else
			{
				newMaterial.Duration = (int)(item.Assignment!.TimeLimit! * systemConfig.Value!);
				var newAssignment = _mapper.Map<Assignment>(item.Assignment);
				newAssignment.Id = Guid.NewGuid().ToString();
				newMaterial.AssignmentId = newAssignment.Id;
				newMaterial.Assignment = newAssignment;
				await _assignmentRepository.Add(newAssignment);
			}
			return hasLearners ? newMaterial : oldMaterial;
		}

	}
}
