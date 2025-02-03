using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.LearningPaths.Queries.GetMyLearningPaths;

public class GetMyLearningPathQuery: IRequest<APIResponse>
{
    public string UserId { get; set; }

    public GetMyLearningPathQuery(string userId)
    {
        UserId = userId;
    }
}
