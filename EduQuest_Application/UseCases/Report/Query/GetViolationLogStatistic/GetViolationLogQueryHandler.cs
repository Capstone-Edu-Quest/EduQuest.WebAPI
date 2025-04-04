using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Report.Query.GetViolationLogStatistic;

public class GetViolationLogQueryHandler : IRequestHandler<GetViolationLogQuery, APIResponse>
{

    public Task<APIResponse> Handle(GetViolationLogQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
