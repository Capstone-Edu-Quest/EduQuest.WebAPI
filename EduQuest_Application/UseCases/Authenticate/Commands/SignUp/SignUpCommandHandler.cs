using AutoMapper;
using EduQuest_Application.Abstractions.Authentication;
using EduQuest_Application.DTO.Response;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Domain.Repository;
using MediatR;

namespace EduQuest_Application.UseCases.Authenticate.Commands.SignUp;

public class SignUpCommandHandler 
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtProvider _jwtProvider;
    private readonly IMapper _mapper;

    public SignUpCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IJwtProvider jwtProvider, IMapper mapper)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _jwtProvider = jwtProvider;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.ConfirmPassword))
        {
            return new APIResponse { IsError = true, Errors = new List<string> { "All fields are required." } };
        }

        if (request.Password != request.ConfirmPassword)
        {
            return new APIResponse { IsError = true, Errors = new List<string> { "Passwords do not match." } };
        }

        var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new APIResponse { IsError = true, Errors = new List<string> { "Email is already taken." } };
        }

        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var newUser = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = request.Email,
            Username = request.Username,
            Status = AccountStatus.Active.ToString(),
            RoleId = ((int)GeneralEnums.UserRole.Learner).ToString(),
            PasswordHash = Convert.ToBase64String(passwordHash),
            PasswordSalt = Convert.ToBase64String(passwordSalt)
        };

        await _userRepository.Add(newUser);
        await _unitOfWork.SaveChangesAsync();

        var tokenResponse = await _jwtProvider.GenerateTokensForUser(newUser.Id, newUser.Email, Guid.NewGuid().ToString());

        var userDTO = _mapper.Map<UserResponseDto>(newUser);

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
