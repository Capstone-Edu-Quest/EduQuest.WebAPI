using AutoMapper;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.LessonContents.Command.UpdateQuiz
{
	public class UpdateQuizCommandHandler : IRequestHandler<UpdateQuizCommand, APIResponse>
	{
		private readonly IQuizRepository _quizRepository;
		private readonly IQuestionRepository _questionRepository;
		private readonly IOptionRepository _answerRepository;
		private readonly ILessonContentRepository _lessonMaterialRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public UpdateQuizCommandHandler(IQuizRepository quizRepository, 
			IQuestionRepository questionRepository, 
			IOptionRepository answerRepository, 
			ILessonContentRepository lessonMaterialRepository, 
			IUnitOfWork unitOfWork, 
			IMapper mapper)
		{
			_quizRepository = quizRepository;
			_questionRepository = questionRepository;
			_answerRepository = answerRepository;
			_lessonMaterialRepository = lessonMaterialRepository;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<APIResponse> Handle(UpdateQuizCommand request, CancellationToken cancellationToken)
		{
			//var isUsed = await _lessonMaterialRepository.IsLessonContentUsed(request.Quiz.Id);
			//if (isUsed)
			//{
			//	return GeneralHelper.CreateErrorResponse(
			//	HttpStatusCode.BadRequest,
			//	MessageCommon.UpdateFailed,
			//	MessageError.UsedContent,
			//	"name",
			//	$"Quiz ID {request.Quiz.Id}"
			//);
			//}

			//// Lấy quiz hiện tại và cập nhật lại
			//var quiz = await _quizRepository.GetQuizById(request.Quiz.Id);
			//if (quiz != null)
			//{
			//	var questions = quiz.Questions.ToList();
			//	foreach (var question in questions)
			//	{

			//		var listAnswer = question.Options.ToList();
			//		_answerRepository.DeleteRange(listAnswer);
			//		await _unitOfWork.SaveChangesAsync();
			//	}

			//	// Xoá toàn bộ question cũ
			//	_questionRepository.DeleteRange(questions);

			//	// Cập nhật lại quiz
			//	_mapper.Map(request.Quiz, quiz);
			//	await _quizRepository.Update(quiz);

			//	await _unitOfWork.SaveChangesAsync();
			//}

			//// Tạo mới câu hỏi
			//var newQuestions = _mapper.Map<List<Question>>(request.Quiz.Questions);
			//foreach (var question in newQuestions)
			//{
			//	question.Id = Guid.NewGuid().ToString();
			//	question.QuizId = quiz!.Id;
			//}
			//await _questionRepository.CreateRangeAsync(newQuestions);

			//// Tạo mới câu trả lời
			//var allOptions = new List<Option>();
			//foreach (var questionRequest in request.Quiz.Questions)
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

			//var result = await _unitOfWork.SaveChangesAsync();
			//if (result > 0)
			//{
			//	return GeneralHelper.CreateSuccessResponse(
			//		HttpStatusCode.OK,
			//		MessageCommon.UpdateSuccesfully,
			//		quiz,
			//		"name",
			//		$"Quiz ID {request.Quiz.Id}"
			//	);
			//}

			return GeneralHelper.CreateErrorResponse(
				HttpStatusCode.BadRequest,
				MessageCommon.UpdateFailed,
				"Saving Failed",
				"name",
				$"Quiz ID {request.Quiz}"
			);
		}
	}
}
