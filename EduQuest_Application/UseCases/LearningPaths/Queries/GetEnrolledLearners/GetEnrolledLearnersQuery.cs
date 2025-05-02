using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.LearningPaths.Queries.GetEnrolledLearners;

public class GetEnrolledLearnersQuery : IRequest<APIResponse>
{
    public string LearningPathId { get; set; }

    public GetEnrolledLearnersQuery(string learningPathId)
    {
        LearningPathId = learningPathId;
    }
}
