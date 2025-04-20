using AutoMapper;
using EduQuest_Application.Abstractions.Authentication;
using EduQuest_Application.DTO.Response;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Domain.Repository;
using MediatR;
using EduQuest_Application.Helper;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using EduQuest_Application.ExternalServices.QuartzService;
using EduQuest_Application.DTO.Response.Users;

namespace EduQuest_Application.UseCases.Authenticate.Commands.SignUp;

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, APIResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtProvider _jwtProvider;
    private readonly IMapper _mapper;
    private readonly IQuartzService _quartzService;

    public SignUpCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IJwtProvider jwtProvider, IMapper mapper, IQuartzService quartzService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _jwtProvider = jwtProvider;
        _mapper = mapper;
        _quartzService = quartzService;
    }

    public async Task<APIResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.FullName) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.ConfirmPassword))
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.LoginFailed, MessageCommon.LoginFailed, "name", "");
        }

        if (request.Password != request.ConfirmPassword)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.WrongPassword, MessageCommon.WrongPassword, "name", "password");
        }

        var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.Conflict, MessageCommon.EmailExisted, MessageCommon.EmailExisted, "name", request.Email ?? "");
        }

        AuthenHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var userId = Guid.NewGuid().ToString();
        var newUser = new User
        {
            Id = userId,
            Email = request.Email,
            Username = request.FullName,
            AvatarUrl = null,
            Status = AccountStatus.Active.ToString(),
            RoleId = ((int)GeneralEnums.UserRole.Learner).ToString(),
            Package = GeneralEnums.PackageEnum.Free.ToString(),
            PasswordHash = Convert.ToBase64String(passwordHash),
            PasswordSalt = Convert.ToBase64String(passwordSalt),
            UserMeta = new UserMeta
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                CurrentStreak = 0,
                LongestStreak = 0,
                TotalCompletedCourses = 0,
                Gold = 0,
                Exp = 0,
                Level = 1,
                TotalStudyTime = 0,
                TotalCourseCreated = 0,
                TotalLearner = 0,
                TotalReview = 0,
                LastActive = DateTime.UtcNow.ToUniversalTime(),
                CreatedAt = DateTime.Now.ToUniversalTime(),
                UpdatedAt = DateTime.Now.ToUniversalTime(),
            },
            FavoriteList = new FavoriteList
            {
                UserId = userId
            }
        };

        await _userRepository.Add(newUser);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var tokenResponse = await _jwtProvider.GenerateTokensForUser(newUser.Id, newUser.Email, Guid.NewGuid().ToString());

        var userDTO = _mapper.Map<UserResponseDto>(newUser);
        await _quartzService.AddAllQuestsToNewUser(userId);
        return new APIResponse
        {
            IsError = false,
            Payload = new LoginResponseDto
            {
                UserData = userDTO,
                Token = tokenResponse
            }
        };
    }
}
