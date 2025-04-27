using EduQuest_Application.Abstractions.Authentication;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.DTO.Response;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using Google.Apis.Auth.OAuth2.Responses;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Users.Commands.SwitchRole;

public class SwitchRoleCommandHandler : IRequestHandler<SwitchRoleCommand, APIResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IRedisCaching _redisCaching;
    private readonly IJwtProvider _jwtProvider;
    private readonly IUnitOfWork _unitOfWork;

    public SwitchRoleCommandHandler(IUserRepository userRepository, IRedisCaching redisCaching, IJwtProvider jwtProvider, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _redisCaching = redisCaching;
        _jwtProvider = jwtProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(SwitchRoleCommand request, CancellationToken cancellationToken)
    {
        //var accessTokenHashKey = $"Token_{request.accessToken}";
        //await _redisCaching.SetAsync(accessTokenHashKey, true, 5);
        var user = await _userRepository.GetById(request.userId);
        if (user == null)
        {
            return GeneralHelper.CreateErrorResponse(
                HttpStatusCode.NotFound,
                MessageCommon.NotFound,
                MessageCommon.NotFound,
                "name",
                request.userId
            );
        }

        user.RoleId = request.RoleId;
        
        await _userRepository.Update(user);

        await _unitOfWork.SaveChangesAsync();
        //var response = await _jwtProvider.GenerateAccessToken(user.Email!);

        return GeneralHelper.CreateSuccessResponse(
            HttpStatusCode.OK,
            MessageCommon.UpdateSuccesfully,
            null,
            "user",
            user.Username
        );
    }
}
