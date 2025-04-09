
using AutoMapper;
using EduQuest_Application.DTO.Response.Users;
using EduQuest_Application.Helper;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Users.Commands.DisableAndEnableUsers;

public class DisableAndEnableUserHandler : IRequestHandler<DisableAndEnableUserCommand, APIResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private const string KeyOperant = "name";
    private const string EntityValue = "user";
    public DisableAndEnableUserHandler(IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(DisableAndEnableUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            #region validate 
            var admin = await _userRepository.GetById(request.AdminId);
            if (admin == null)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.Unauthorized, MessageCommon.Unauthorized,
                    MessageCommon.UpdateFailed, KeyOperant, EntityValue);
            }
            var user = await _userRepository.GetById(request.UserId);
            if (user == null)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.NotFound,
                    MessageCommon.UpdateFailed, KeyOperant, EntityValue);
            }
            #endregion

            user.Status = AccountStatus.Blocked.ToString();
            await _userRepository.Update(user);

            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                UserResponseDto response = _mapper.Map<UserResponseDto>(user);
                return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.UpdateSuccesfully, response, KeyOperant, EntityValue);
            }
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed,
                    MessageCommon.UpdateFailed, KeyOperant, EntityValue);
        }
        catch (Exception ex)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message,
                    MessageCommon.UpdateFailed, KeyOperant, EntityValue);
        }
    }
}
