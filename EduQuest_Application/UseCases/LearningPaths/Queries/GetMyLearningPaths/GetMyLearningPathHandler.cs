using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.LearningPaths.Queries.GetMyLearningPaths;

public class GetMyLearningPathHandler : IRequestHandler<GetMyLearningPathQuery, APIResponse>
{
    public Task<APIResponse> Handle(GetMyLearningPathQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
