

using AutoMapper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using EduQuest_Application.DTO.Response.Feedbacks;
using EduQuest_Application.Helper;
using EduQuest_Domain.Enums;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.Feedbacks.Commands.DeteleFeedback;

public class DeleteFeedbackHandler : IRequestHandler<DeteleFeedbackCommand, APIResponse>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private const string Key = "name";
    private const string value = "feedback";

    public DeleteFeedbackHandler(IFeedbackRepository feedbackRepository, IMapper mapper, 
        IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _feedbackRepository = feedbackRepository;
        _mapper = mapper;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(DeteleFeedbackCommand request, CancellationToken cancellationToken)
    {
        try
        {
            #region validate owner
            bool isOwner = await _feedbackRepository.IsOnwer(request.FeedbackId, request.UserId);
            var user = await _userRepository.GetById(request.UserId);
            if (!isOwner || user != null && Convert.ToInt32(user.RoleId) != (int)UserRole.Staff)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.DeleteFailed, MessageCommon.UserDontHavePer, Key, value);
            }
            #endregion

            var feedback = await _feedbackRepository.GetById(request.FeedbackId);
            if (feedback == null)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.DeleteFailed, MessageCommon.NotFound, Key, value);
            }

            await _feedbackRepository.Delete(feedback);

            #region return value
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.DeleteSuccessfully,
                    _mapper.Map<FeedbackResponse>(feedback), Key, value);
            }

            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.DeleteFailed, MessageCommon.DeleteFailed, Key, value);
            #endregion
        }
        catch (Exception ex)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed, ex.Message, Key, value);
        }
    }
}
