using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Transactions.Query.GetTransactionByFilter;

public class GetTransactionByFilterQuery : IRequest<APIResponse>
{
    public string? TransactionId { get; set; }
    public string? UserId { get; set; }
    public string? CourseId { get; set; }
    public string? Status { get; set; }
    public string? Type { get; set; }
}
