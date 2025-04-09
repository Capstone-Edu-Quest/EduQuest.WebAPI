using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Coupons.Queries.GetByCode;

public class GetByCodeCommand : IRequest<APIResponse>
{
    public string Code { get; set; }
}
