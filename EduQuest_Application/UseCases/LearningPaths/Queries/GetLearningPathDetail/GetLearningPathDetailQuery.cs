

using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.LearningPaths.Queries.GetLearningPathDetail;

public class GetLearningPathDetailQuery : IRequest<APIResponse>
{
    public string LearningPathId { get; set; }
    public string UserId { get; set; }


    public GetLearningPathDetailQuery(string learningPathId, string userId)
    {
        LearningPathId = learningPathId;
        UserId = userId;
    }
}
