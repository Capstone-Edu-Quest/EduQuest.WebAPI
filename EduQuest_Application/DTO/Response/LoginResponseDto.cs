using EduQuest_Application.DTO.Response.Users;

namespace EduQuest_Application.DTO.Response;

public class LoginResponseDto
{
    public UserResponseDto? UserData { get; set; }
    public TokenResponseDTO? Token { get; set; }
}
