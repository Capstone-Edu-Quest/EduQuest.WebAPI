using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Authenticate.Commands.LogOut;

public class SignOutCommandHandler : IRequestHandler<SignOutCommand, APIResponse>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IRedisCaching _redisCaching;
    private readonly IUnitOfWork _unitOfWork;

    public SignOutCommandHandler(IRefreshTokenRepository refreshTokenRepository, IRedisCaching redisCaching, IUnitOfWork unitOfWork)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _redisCaching = redisCaching;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        //var accessTokenHashKey = $"Token_{request.accessToken}";
        //await _redisCaching.SetAsync(accessTokenHashKey, true, 5);
        var tokenId = AuthenHelper.ExtractIdFromRefreshToken(request.refreshToken);
        var tokenEntity = await _refreshTokenRepository.GetById(tokenId);
        if (tokenEntity == null)
        {
            return new APIResponse
            {
                IsError = true,
                Errors = new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = MessageCommon.NotFound
                },
                Payload = null,
                Message = new MessageResponse
                {
                    content = MessageCommon.NotFound
                }
            };
        }

        await _refreshTokenRepository.Delete(tokenEntity.Id);

        await _unitOfWork.SaveChangesAsync();

        return new APIResponse
        {
            IsError = false,
            Errors = null,
            Payload = null,
            Message = new MessageResponse { content = MessageCommon.LogOutSuccessfully }
        };
    }
}

