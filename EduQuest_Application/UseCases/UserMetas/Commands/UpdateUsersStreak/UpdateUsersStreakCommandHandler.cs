using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.Generic;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.UserMetas.Commands.UpdateUsersStreak;

public class UpdateUsersStreakCommandHandler : IRequestHandler<UpdateUsersStreakCommand, APIResponse>
{
    public IUserMetaRepository userMetaRepository;
    public IUnitOfWork _unitOfWork;

    public UpdateUsersStreakCommandHandler(IUserMetaRepository userMetaRepository, IUnitOfWork unitOfWork)
    {
        this.userMetaRepository = userMetaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(UpdateUsersStreakCommand request, CancellationToken cancellationToken)
    {
        var existUser = await userMetaRepository.GetByUserId(request.UserId);
        if (existUser == null)
        {
            return new APIResponse
            {
                IsError = true,
                Errors = new ErrorResponse
                {
                    Message = MessageCommon.NotFound,
                    StatusCode = (int)HttpStatusCode.NotFound,
                },
                Message = new MessageResponse
                {
                    content = MessageCommon.NotFound,
                    values = new { name = "user" }
                }
            };
        }

        if (existUser.LastLearningDay == null)
        {
            existUser.LastLearningDay = DateTime.UtcNow.ToUniversalTime();
        }

        DateTime lastLearningDay = existUser.LastLearningDay.Value.Date;

        existUser.CurrentStreak = (lastLearningDay == DateTime.UtcNow.Date.AddDays(-1)) ? existUser.CurrentStreak + 1 : 1;
        existUser.LastLearningDay = DateTime.UtcNow.ToUniversalTime();
        existUser.LongestStreak = Math.Max((byte)existUser.LongestStreak!, (byte)existUser.CurrentStreak!);

        await userMetaRepository.Update(existUser);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new APIResponse
        {
            IsError = false,
            Message = new MessageResponse
            {
                content = MessageCommon.UpdateSuccesfully,
                values = new { name = "streak" }
            }
        };


    }
}
