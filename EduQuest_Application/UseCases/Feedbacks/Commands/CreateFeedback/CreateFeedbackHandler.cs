using AutoMapper;
using EduQuest_Application.DTO.Response.Feedbacks;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Feedbacks.Commands.CreateFeedback;

public class CreateFeedbackHandler : IRequestHandler<CreateFeedbackCommand, APIResponse>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateFeedbackHandler(IFeedbackRepository feedbackRepository, IUserRepository userRepository, 
        IMapper mapper, IUnitOfWork unitOfWork)
    {
        _feedbackRepository = feedbackRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // validate user
            var user = await _userRepository.GetById(request.UserId);
            if (user == null)
            {
                return new APIResponse
                {
                    IsError = true,
                    Payload = null,
                    Errors = new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                        StatusResponse = HttpStatusCode.Unauthorized,
                        Message = MessageCommon.SessionTimeout
                    },
                    Message = new MessageResponse
                    {
                        content = MessageCommon.CreateFailed,
                        values = new Dictionary<string, string> { { "name", "feedback" } }
                    }
                };
            }

            Feedback newFeedback = _mapper.Map<Feedback>(request.Feedback);
            newFeedback.UserId = request.UserId;

            await _feedbackRepository.Add(newFeedback);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return new APIResponse
                {
                    IsError = false,
                    Payload = _mapper.Map<FeedbackResponse>(newFeedback),
                    Errors = null,
                    Message = new MessageResponse
                    {
                        content = MessageCommon.CreateSuccesfully,
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
                    Message = MessageCommon.CreateFailed
                },
                Message = new MessageResponse
                {
                    content = MessageCommon.CreateFailed,
                    values = new Dictionary<string, string> { { "name", "feedback" } }
                }
            };
        }catch(Exception ex)
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
                    content = MessageCommon.CreateFailed,
                    values = new Dictionary<string, string> { { "name", "feedback" } }
                }
            };
        }
    }
}
