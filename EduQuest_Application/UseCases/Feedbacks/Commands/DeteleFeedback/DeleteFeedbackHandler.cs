

using AutoMapper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using EduQuest_Application.DTO.Response.Feedbacks;

namespace EduQuest_Application.UseCases.Feedbacks.Commands.DeteleFeedback;

public class DeleteFeedbackHandler : IRequestHandler<DeteleFeedbackCommand, APIResponse>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFeedbackHandler(IFeedbackRepository feedbackRepository, IMapper mapper, 
        IUnitOfWork unitOfWork)
    {
        _feedbackRepository = feedbackRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(DeteleFeedbackCommand request, CancellationToken cancellationToken)
    {
        try
        {
            #region validate owner
            bool isOwner = await _feedbackRepository.IsOnwer(request.FeedbackId, request.UserId);
            if (!isOwner)
            {
                return new APIResponse
                {
                    IsError = true,
                    Payload = null,
                    Errors = new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        StatusResponse = HttpStatusCode.BadRequest,
                        Message = MessageCommon.UserDontHavePer
                    },
                    Message = new MessageResponse
                    {
                        content = MessageCommon.UpdateFailed,
                        values = new Dictionary<string, string> { { "name", "feedback" } }
                    }
                };
            }
            #endregion

            var feedback = await _feedbackRepository.GetById(request.FeedbackId);
            if (feedback == null)
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
                        content = MessageCommon.UpdateFailed,
                        values = new Dictionary<string, string> { { "name", "feedback" } }
                    }
                };
            }

            await _feedbackRepository.Delete(feedback);

            #region return value
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return new APIResponse
                {
                    IsError = false,
                    Payload = _mapper.Map<FeedbackResponse>(feedback),
                    Errors = null,
                    Message = new MessageResponse
                    {
                        content = MessageCommon.DeleteSuccessfully,
                        values = new Dictionary<string, string> { { "name", "feedback" } }
                    }
                };
            }

            return new APIResponse
            {
                IsError = true,
                Payload = null,
                Errors = new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = MessageCommon.DeleteFailed
                },
                Message = new MessageResponse
                {
                    content = MessageCommon.UpdateFailed,
                    values = new Dictionary<string, string> { { "name", "feedback" } }
                }
            };
            #endregion
        }
        catch (Exception ex)
        {
            return new APIResponse
            {
                IsError = true,
                Payload = null,
                Errors = new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    StatusResponse = HttpStatusCode.BadRequest,
                    Message = ex.Message
                },
                Message = new MessageResponse
                {
                    content = MessageCommon.DeleteFailed,
                    values = new Dictionary<string, string> { { "name", "feedback" } }
                }
            };
        }
    }
}
