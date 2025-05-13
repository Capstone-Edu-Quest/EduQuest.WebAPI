using AutoMapper;
using EduQuest_Application.DTO.Response.Materials.DetailMaterials;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.LessonContents.Query.GetQuixById
{
	public class GetQuizByIdQueryHandler : IRequestHandler<GetQuizByIdQuery, APIResponse>
	{
		private readonly IQuizRepository _quizRepository;
		private readonly IMapper _mapper;

		public GetQuizByIdQueryHandler(IQuizRepository quizRepository, IMapper mapper)
		{
			_quizRepository = quizRepository;
			_mapper = mapper;
		}

		public async Task<APIResponse> Handle(GetQuizByIdQuery request, CancellationToken cancellationToken)
		{
			var existedQuiz = await _quizRepository.GetById(request.QuizId);
            if (existedQuiz == null)
			{
				return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, MessageCommon.NotFound, MessageCommon.NotFound, "name", $"Quiz ID {request.QuizId}");
			}
           
            var response = _mapper.Map<QuizTypeDto>(existedQuiz);
			return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, response, "name", $"Quiz ID {request.QuizId}");
		}
	}
}
