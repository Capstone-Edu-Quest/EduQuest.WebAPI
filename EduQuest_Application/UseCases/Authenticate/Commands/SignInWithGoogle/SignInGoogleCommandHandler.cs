using AutoMapper;
using EduQuest_Application.Abstractions.Authentication;
using EduQuest_Application.Abstractions.Oauth2;
using EduQuest_Application.DTO.Response;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Oauth2;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;

namespace Application.UseCases.Authenticate.Commands.SignInWithGoogle
{
    public class SignInGoogleCommandHandler : IRequestHandler<SignInGoogleCommand, APIResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenValidation _googleTokenValidation;
        private readonly IMapper _mapper;
        private readonly IJwtProvider _jwtProvider;

        public SignInGoogleCommandHandler(IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork,
            ITokenValidation googleTokenValidation,
            IMapper mapper,
            IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _googleTokenValidation = googleTokenValidation;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
        }

        public async Task<APIResponse> Handle(SignInGoogleCommand request, CancellationToken cancellationToken)
        {
            var tokenValidationResponse = await _googleTokenValidation.ValidateGoogleTokenAsync(request.Token!);
            if (tokenValidationResponse.IsError)
            {
                return tokenValidationResponse;
            }

            var tokenInfo = tokenValidationResponse.Payload as GoogleTokenInfo;

            var user = await _userRepository.GetUserByEmailAsync(tokenInfo!.Email!);
            if (user == null)
            {
                var newUser = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = tokenInfo!.Email,
                    Username = tokenInfo.FullName,
                    AvatarUrl = tokenInfo.picture,
                    Status = AccountStatus.Active.ToString(),
                    RoleId = "1",
                };

                await _userRepository.Add(newUser);

                await _unitOfWork.SaveChangesAsync();

                var response = await _jwtProvider.GenerateAccessRefreshTokens(newUser.Id, newUser.Email!);

                await _unitOfWork.SaveChangesAsync();

                var data = _mapper.Map<UserResponseDto>(newUser);
                return new APIResponse
                {
                    IsError = false,
                    Errors = null,
                    Payload = new LoginResponseDto
                    {
                        UserData = data,
                        Token = response
                    }
                };

            };


            //check if the refresh token exists, then remove it to create new refresh token
            var existingRefreshTokens = await _refreshTokenRepository.GetUserByIdAsync(user.Id);
            if (existingRefreshTokens != null)
            {
                await _refreshTokenRepository.Delete(existingRefreshTokens.Id);
            }
            //create new refresh token
            var tokenResponse = await _jwtProvider.GenerateAccessRefreshTokens(user.Id, user.Email!);
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
    }
}
