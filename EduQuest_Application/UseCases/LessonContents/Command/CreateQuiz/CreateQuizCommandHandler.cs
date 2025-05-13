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

			// Add new Questions for the quiz
			var questions = _mapper.Map<List<Question>>(request.Quiz.Questions);
			foreach (var question in questions)
			{
				question.Id = Guid.NewGuid().ToString();
				question.QuizId = newQuiz.Id;
			}
			await _questionRepository.CreateRangeAsync(questions);

			// Add answers for the questions
			var answers = new List<Option>();
			foreach (var questionRequest in request.Quiz.Questions)
			{
				var question = questions.First(q => q.QuestionTitle == questionRequest.QuestionTitle);
				var answersForQuestion = _mapper.Map<List<Option>>(questionRequest.Options);

				foreach (var answer in answersForQuestion)
				{
					answer.Id = Guid.NewGuid().ToString();
					answer.QuestionId = question.Id;
				}

				answers.AddRange(answersForQuestion);
			}

			await _answerRepository.CreateRangeAsync(answers);
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
