using EduQuest_Domain.Models.Response;
using MediatR;


namespace EduQuest_Application.UseCases.Feedbacks.Commands.DeteleFeedback;

public class DeteleFeedbackCommand : IRequest<APIResponse>
{
    public string UserId { get; set; }
    public string FeedbackId { get; set; }

    public DeteleFeedbackCommand(string userId, string feedbackId)
    {
        UserId = userId;
        FeedbackId = feedbackId;
    }
}
