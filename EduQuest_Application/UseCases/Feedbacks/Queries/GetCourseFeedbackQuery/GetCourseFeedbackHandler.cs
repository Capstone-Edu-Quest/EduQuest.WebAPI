using AutoMapper;
using EduQuest_Application.DTO.Response.Feedbacks;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Constants.Constants;
using static Google.Rpc.Context.AttributeContext.Types;

namespace EduQuest_Application.UseCases.Feedbacks.Queries.GetCourseFeedbackQuery;

public class GetCourseFeedbackHandler : IRequestHandler<GetCourseFeedbackQuery, APIResponse>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IMapper _mapper;

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
                CommonUserResponse userResponse = _mapper.Map<CommonUserResponse>(item.User);
                FeedbackResponse myFeedbackResponse = _mapper.Map<FeedbackResponse>(item);
                myFeedbackResponse.CreatedBy = userResponse;
                responseDto.Add(myFeedbackResponse);
            }
            PagedList<FeedbackResponse> responses = new PagedList<FeedbackResponse>(responseDto, result.TotalItems, result.CurrentPage, result.EachPage);
            return new APIResponse
            {
                IsError = true,
                Payload = responses,
                Errors = null,
                Message = new MessageResponse
                {
                    content = MessageCommon.GetSuccesfully,
                    values = new Dictionary<string, string> { { "name", "feedback" } }
                }
            };
        }catch (Exception ex)
        {
            return new APIResponse
            {
                IsError = false,
                Payload = null,
                Errors = new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = ex.Message
                },
                Message = new MessageResponse
                {
                    content = MessageCommon.GetFailed,
                    values = new Dictionary<string, string> { { "name", "feedback" } }
                }
            };
        }
    }
}
