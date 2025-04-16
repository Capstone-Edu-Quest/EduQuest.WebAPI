using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Users.Queries.GetUserByStatus;

public class GetUserByStatusQueryHandler : IRequestHandler<GetUserByStatus, APIResponse>
{
    public Task<APIResponse> Handle(GetUserByStatus request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
