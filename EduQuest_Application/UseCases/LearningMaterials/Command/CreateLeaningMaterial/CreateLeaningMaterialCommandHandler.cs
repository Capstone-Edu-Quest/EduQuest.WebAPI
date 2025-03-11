using AutoMapper;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.LearningMaterials.Command.CreateLeaningMaterial
{
    public class CreateLeaningMaterialCommandHandler : IRequestHandler<CreateLeaningMaterialCommand, APIResponse>
    {
        private readonly ILearningMaterialRepository _learningMaterialRepository;
        private readonly ISystemConfigRepository _systemConfigRepository;
        private readonly IStageRepository _stageRepository;
        private readonly IAssignmentRepository _assignmentRepository;
		private readonly IQuizRepository _quizRepository;
		private readonly IQuestionRepository _questionRepository;
		private readonly IAnswerRepository _answerRepository;
		private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

		public CreateLeaningMaterialCommandHandler(ILearningMaterialRepository learningMaterialRepository, 
			ISystemConfigRepository systemConfigRepository, 
			IStageRepository stageRepository, 
			IAssignmentRepository assignmentRepository, 
			IQuizRepository quizRepository, 
			IQuestionRepository questionRepository, 
			IAnswerRepository answerRepository, 
			IMapper mapper, 
			IUnitOfWork unitOfWork)
		{
			_learningMaterialRepository = learningMaterialRepository;
			_systemConfigRepository = systemConfigRepository;
			_stageRepository = stageRepository;
			_assignmentRepository = assignmentRepository;
			_quizRepository = quizRepository;
			_questionRepository = questionRepository;
			_answerRepository = answerRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<APIResponse> Handle(CreateLeaningMaterialCommand request, CancellationToken cancellationToken)
        {
            var apiResponse = new APIResponse();
            if (request.Material == null || !request.Material.Any())
            {
                return apiResponse = GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, MessageCommon.NotFound,"Empty", "name", "Course");
			}
            var listMaterial = new List<Material>();
			

			foreach (var item in request.Material)
            {
				var material = _mapper.Map<Material>(item);
				material.Id = Guid.NewGuid().ToString();
				material.Type = Enum.GetName(typeof(TypeOfLearningMetarial), item.Type);

				var value = await _systemConfigRepository.GetByName(material.Type!);
				switch ((TypeOfLearningMetarial)item.Type!)
				{
					case TypeOfLearningMetarial.Document:
						material.Duration = (int)value.Value!;
                        material.Content = item.Content;
						break;
					case TypeOfLearningMetarial.Video:
						material.Duration = item.VideoRequest!.Duration;
                        material.UrlMaterial = item.VideoRequest.UrlMaterial;
                        material.Thumbnail = item.VideoRequest.Thumbnail;
						break;
					case TypeOfLearningMetarial.Quiz:
						material.Duration = (int)(item.QuizRequest!.TimeLimit! * value.Value!);

                        //Add new Quiz
                        var newQuiz = _mapper.Map<Quiz>(item.QuizRequest!);
                        newQuiz.Id = Guid.NewGuid().ToString(); 
                        newQuiz.CreatedBy = request.UserId;
                        await _quizRepository.Add(newQuiz);
                        

                        //Add new Questions for above quiz
                        var newQuestions = _mapper.Map<List<Question>>(item.QuizRequest.QuestionRequest);
                        foreach(var question in newQuestions)
                        {
                            question.Id = Guid.NewGuid().ToString();
                            question.QuizId = newQuiz.Id;
                            question.MultipleAnswers = question.MultipleAnswers;
						}
						await _questionRepository.CreateRangeAsync(newQuestions);

						//Add new answer for list question above
						var newAnswers = new List<Answer>();
						foreach (var questionRequest in item.QuizRequest.QuestionRequest)
						{
							var question = newQuestions.First(q => q.QuestionTitle == questionRequest.QuestionTitle);
							var answers = _mapper.Map<List<Answer>>(questionRequest.AnswerRequest);

							foreach (var answer in answers)
							{
								answer.Id = Guid.NewGuid().ToString();
								answer.QuestionId = question.Id;
							}

							newAnswers.AddRange(answers);
						}

						await _answerRepository.CreateRangeAsync(newAnswers);
						await _unitOfWork.SaveChangesAsync();
						break;
					case TypeOfLearningMetarial.Assignment:
						material.Duration = (int)(item.AssignmentRequest!.TimeLimit! * value.Value!);
                        var newAssignment = _mapper.Map<Assignment>(item.AssignmentRequest);
                        newAssignment.Id = Guid.NewGuid().ToString();
						break;
					default:
						material.Duration = 0;
						break;
				}
				await _learningMaterialRepository.Add(material);
				listMaterial.Add(material);

            }
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
    }
}
