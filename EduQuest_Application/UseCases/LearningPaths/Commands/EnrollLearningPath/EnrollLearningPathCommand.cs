using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.LearningPaths.Commands.EnrollLearningPath;

public class EnrollLearningPathCommand : IRequest<APIResponse>
{
    public string LearningPathId { get; set; }
    public string UserId { get; set; }

    public EnrollLearningPathCommand(string learningPathId, string userId)
    {
        LearningPathId = learningPathId;
        UserId = userId;
    }
}
