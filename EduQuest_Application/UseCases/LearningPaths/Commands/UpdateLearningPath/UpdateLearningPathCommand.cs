

using EduQuest_Application.DTO.Request.LearningPaths;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.LearningPaths.Commands.UpdateLearningPath;

public class UpdateLearningPathCommand : IRequest<APIResponse>
{
    public string LearningPathId { get; set; }
    public string UserId { get; set; }
    public UpdateLearningPathRequest LearningPathRequest { get; set; }

    public UpdateLearningPathCommand(string learningPathId, string userId, UpdateLearningPathRequest learningPathRequest)
    {
        LearningPathId = learningPathId;
        UserId = userId;
        LearningPathRequest = learningPathRequest;
    }
}
