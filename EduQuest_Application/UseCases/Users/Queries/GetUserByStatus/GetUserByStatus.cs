using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Users.Queries.GetUserByStatus;

public class GetUserByStatus : IRequest<APIResponse>
{
    public string status { get; set; }
}
