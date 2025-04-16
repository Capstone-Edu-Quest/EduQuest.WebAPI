using AutoMapper;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Courses.Query.GetQuizAttempts;

public class GetQuizAttemptsHandler : IRequestHandler<GetQuizAttemptsQuery, APIResponse>
{
    private readonly IQuizAttemptRepository _quizAttemptRepository;
    private readonly IMapper _mapper;

    public GetQuizAttemptsHandler(IQuizAttemptRepository quizAttemptRepository, IMapper mapper)
    {
        _quizAttemptRepository = quizAttemptRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetQuizAttemptsQuery request, CancellationToken cancellationToken)
    {
        var result = await _quizAttemptRepository.GetQuizAttempts(request.QuizId, request.LessonId, request.UserId);
        List<QuizAttemptsResponse> responses = _mapper.Map<List<QuizAttemptsResponse>>(result);

        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully,
            responses, "name", "quiz attempts");
    }
}
