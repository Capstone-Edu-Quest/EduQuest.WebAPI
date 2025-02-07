using AutoMapper;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.LearningPaths.Queries.GetMyPublicLearningPaths;

public class GetMyPublicLearningPathHandler : IRequestHandler<GetMyPublicLearningPathQuery, APIResponse>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly IMapper _mapper;

    public GetMyPublicLearningPathHandler(ILearningPathRepository learningPathRepository, IMapper mapper)
    {
        _learningPathRepository = learningPathRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetMyPublicLearningPathQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _learningPathRepository.GetMyPublicLearningPaths(request.UserId);
            List<MyPublicLearningPathResponse> responseDto = new List<MyPublicLearningPathResponse>();
            foreach (var item in result)
            {
                MyPublicLearningPathResponse myLearningPathResponse = _mapper.Map<MyPublicLearningPathResponse>(item);
                myLearningPathResponse.TotalCourse = item.LearningPathCourses.Count;
                responseDto.Add(myLearningPathResponse);
            }
            return new APIResponse
            {
                IsError = false,
                Payload = responseDto,
                Errors = null,
                Message = new MessageResponse
                {
                    content = MessageCommon.GetSuccesfully,
                    values = new Dictionary<string, string> { { "name", "learning path" } }
                }
            };
        }catch (Exception ex)
        {
            return new APIResponse
            {
                IsError = true,
                Payload = null,
                Errors = new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = ex.Message,
                    StatusResponse = HttpStatusCode.BadRequest
                },
                Message = new MessageResponse
                {
                    content = MessageCommon.GetFailed,
                    values = new Dictionary<string, string> { { "name", "learning path" } }
                }
            };
        }
    }
}
