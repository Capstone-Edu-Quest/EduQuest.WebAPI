using EduQuest_Application.DTO.Request.LearningPaths;
using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.LearningPaths.Commands.CreateLearningPath;

public class CreateLearningPathCommand: IRequest<APIResponse>
{
    public CreateLearningPathRequest CreateLearningPathRequest { get; set; }
    public string UserId { get; set; }

    public CreateLearningPathCommand(CreateLearningPathRequest createLearningPathRequest, string userId)
    {
        CreateLearningPathRequest = createLearningPathRequest;
        UserId = userId;
    }
}
