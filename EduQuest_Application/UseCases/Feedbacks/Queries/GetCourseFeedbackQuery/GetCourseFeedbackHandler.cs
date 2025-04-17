using AutoMapper;
using EduQuest_Application.DTO.Response.Feedbacks;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Feedbacks.Queries.GetCourseFeedbackQuery;

public class GetCourseFeedbackHandler : IRequestHandler<GetCourseFeedbackQuery, APIResponse>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IMapper _mapper;
    private const string Key = "name";
    private const string value = "feedback";
    public GetCourseFeedbackHandler(IFeedbackRepository feedbackRepository, IMapper mapper)
    {
        _feedbackRepository = feedbackRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetCourseFeedbackQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _feedbackRepository.GetByCourseId(request.courseId, request.PageNo, request.PageSize, request.Rating, request.Feedback);

            /*if (result == null)
            {
                return new APIResponse
                {
                    IsError = true,
                    Payload = null,
                    Errors = new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        StatusResponse = HttpStatusCode.BadRequest,
                        Message = MessageCommon.NotFound
                    },
                    Message = new MessageResponse
                    {
                        content = MessageCommon.GetFailed,
                        values = new Dictionary<string, string> { { "name", "feedback" } }
                    }
                };
            }*/

            List<FeedbackResponse> responseDto = new List<FeedbackResponse>();
            var temp = result.Items.ToList();
            foreach (var item in temp)
            {
                FeedbackResponse myFeedbackResponse = _mapper.Map<FeedbackResponse>(item);
                responseDto.Add(myFeedbackResponse);
            }
            PagedList<FeedbackResponse> responses = new PagedList<FeedbackResponse>(responseDto, result.TotalItems, result.CurrentPage, result.EachPage);
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.GetSuccesfully, responses, Key, value);
        }catch (Exception ex)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.GetFailed, ex.Message, Key, value);
        }
    }
}
