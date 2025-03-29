using AutoMapper;
using EduQuest_Application.Abstractions.Authentication;
using EduQuest_Application.DTO.Response;
using EduQuest_Application.Helper;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Authenticate.Commands.SignInWithPassword
{
    public class SignInWithPasswordHandler : IRequestHandler<SignInWithPassword, APIResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtProvider _jwtProvider;
        private readonly IMapper _mapper;

        public SignInWithPasswordHandler(IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork,
            IJwtProvider jwtProvider,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _jwtProvider = jwtProvider;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(SignInWithPassword request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, Constants.MessageCommon.NotFound, MessageCommon.NotFound, "name", "user");
            }

            if (!AuthenHelper.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return GeneralHelper.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, Constants.MessageCommon.invalidEmailOrPassword, MessageCommon.invalidEmailOrPassword, "name", "");
            }


            //var existingRefreshToken = await _refreshTokenRepository.GetByUserIdAndDevice(deviceId);
            var tokens = await _jwtProvider.GenerateTokensForUser(user.Id, user.Email, Guid.NewGuid().ToString());

            await _unitOfWork.SaveChangesAsync();

            var userDTO = _mapper.Map<UserResponseDto>(user);
            return new APIResponse
            {
                IsError = false,
                Errors = null,
                Payload = new LoginResponseDto
                {
                    UserData = userDTO,
                    Token = new TokenResponseDTO
                    {
                        AccessToken = tokens.AccessToken,
                        RefreshToken = tokens.RefreshToken,
                    }
                }
            };
        }

    }
}
