using AutoMapper;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.LessonContents.Command.CreateQuiz
{
	public class CreateQuizCommandHandler : IRequestHandler<CreateQuizCommand, APIResponse>
	{
		private readonly IQuizRepository _quizRepository;
		private readonly IQuestionRepository _questionRepository;
		private readonly IOptionRepository _answerRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public CreateQuizCommandHandler(IQuizRepository quizRepository, IQuestionRepository questionRepository, IOptionRepository answerRepository, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_quizRepository = quizRepository;
			_questionRepository = questionRepository;
			_answerRepository = answerRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<APIResponse> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
		{
			var newQuiz = _mapper.Map<Quiz>(request.Quiz);
			newQuiz.Id = Guid.NewGuid().ToString();
			newQuiz.UserId = request.UserId;
			await _quizRepository.Add(newQuiz);

			foreach(var question in newQuiz.Questions)
			{
				question.CreatedAt = DateTime.Now.ToUniversalTime();
				question.UpdatedAt = DateTime.Now.ToUniversalTime();
				foreach(var option in question.Options)
				{
					option.CreatedAt = DateTime.Now.ToUniversalTime();
					option.UpdatedAt = DateTime.Now.ToUniversalTime();
				}
			}

			//var allAnswers = new List<Option>();
			//var allQuestions = new List<Question>();

			//foreach (var questionRequest in request.Quiz.Questions)
			//{
			//	var question = _mapper.Map<Question>(questionRequest);
			//	question.Id = Guid.NewGuid().ToString();
			//	question.QuizId = newQuiz.Id;
			//	allQuestions.Add(question);	

			//	var answersForQuestion = _mapper.Map<List<Option>>(questionRequest.Options);
			//	foreach (var answer in answersForQuestion)
			//	{
			//		answer.Id = Guid.NewGuid().ToString();
			//		answer.QuestionId = question.Id;
			//	}

			//	allAnswers.AddRange(answersForQuestion);
			//}

			//await _questionRepository.CreateRangeAsync(allQuestions);
			//await _answerRepository.CreateRangeAsync(allAnswers);

			var result = await _unitOfWork.SaveChangesAsync();
			if (result > 0)
			{
				return GeneralHelper.CreateSuccessResponse(
					HttpStatusCode.OK,
					MessageCommon.CreateSuccesfully,
					newQuiz,
					"name",
					"New Quiz"
				);
			}

			return GeneralHelper.CreateErrorResponse(
				HttpStatusCode.BadRequest,
				MessageCommon.CreateFailed,
				"Saving Failed",
				"name",
				"New Quiz"
			);
		}
	}
}
