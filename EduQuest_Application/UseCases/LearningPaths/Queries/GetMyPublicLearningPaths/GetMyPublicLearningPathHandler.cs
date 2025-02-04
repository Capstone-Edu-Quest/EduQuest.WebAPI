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

            return new APIResponse
            {
                IsError = false,
                Payload = _mapper.Map<List<MyPublicLearningPathResponse>>(result),
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
