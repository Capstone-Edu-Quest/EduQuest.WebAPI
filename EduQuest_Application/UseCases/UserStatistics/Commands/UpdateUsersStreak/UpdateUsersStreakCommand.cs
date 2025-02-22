using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.UserStatistics.Commands.UpdateUsersStreak;

public class UpdateUsersStreakCommand : IRequest<APIResponse>
{
    public string UserId { get; set; }
}
