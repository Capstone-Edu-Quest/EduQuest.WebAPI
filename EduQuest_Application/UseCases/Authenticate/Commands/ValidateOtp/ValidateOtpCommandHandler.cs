using AutoMapper;
using EduQuest_Application.Abstractions.Authentication;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.DTO.Response;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.Authenticate.Commands.VerifyPassword;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace Application.UseCases.Authenticate.Queries.ValidateOTP
{
    public class ValidateOtpCommandHandler : IRequestHandler<ValidateOtp, APIResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IRedisCaching _redisCaching;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IMapper _mapper;

        public ValidateOtpCommandHandler(IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IRedisCaching redisCaching,
            IRefreshTokenRepository refreshTokenRepository,
            IJwtProvider jwtProvider,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _redisCaching = redisCaching;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtProvider = jwtProvider;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(ValidateOtp request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email!);
            if (user == null)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.NotFound, MessageCommon.NotFound, "name", "user");

            }

            var existOTP = await _redisCaching.GetAsync<string>($"ResetPassword_{request.Email}");
            if (existOTP == request.Otp)
            {
                var existingRefreshTokens = await _refreshTokenRepository.GetRefreshTokenByUserId(user.Id);
                if (existingRefreshTokens != null)
                {
                    foreach (var token in existingRefreshTokens)
                    {
                        await _refreshTokenRepository.Delete(token!);
                    }
                }

                var tokenResponse = await _jwtProvider.GenerateTokensForUser(user.Id, user.Email!, Guid.NewGuid().ToString());

                await _redisCaching.RemoveAsync($"ResetPassword_{request.Email}");
                await _userRepository.Update(user);
                await _unitOfWork.SaveChangesAsync();

                var userDTO = _mapper.Map<UserResponseDto>(user);
                return new APIResponse
                {
                    IsError = false,
                    Errors = null,
                    Payload = new LoginResponseDto
                    {
                        UserData = userDTO,
                        Token = tokenResponse
                    }
                };
            }

            return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.LoginFailed, MessageCommon.LoginFailed, "name", "user");
        }
    }
}
