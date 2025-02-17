using EduQuest_Application.DTO.Request.Feedbacks;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Feedbacks.Commands.CreateFeedback;

public class CreateFeedbackCommand : IRequest<APIResponse>
{
    public string UserId { get; set; }
    public CreateFeedbackRequest Feedback { get; set; }

    public CreateFeedbackCommand(CreateFeedbackRequest feedback, string userId)
    {
        Feedback = feedback;
        UserId = userId;
    }
}
