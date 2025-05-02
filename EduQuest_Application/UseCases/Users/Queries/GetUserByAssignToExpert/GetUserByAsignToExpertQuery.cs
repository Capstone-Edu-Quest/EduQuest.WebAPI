using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Users.Queries.GetUserByAssignToExpert;

public class GetUserByAsignToExpertQuery : IRequest<APIResponse>
{
    public string expertId { get; set; }
    //public string tagId { get; set; }

}
