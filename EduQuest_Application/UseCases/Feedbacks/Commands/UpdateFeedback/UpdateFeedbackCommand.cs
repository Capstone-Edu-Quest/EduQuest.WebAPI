using EduQuest_Application.DTO.Request.Feedbacks;
using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Feedbacks.Commands.UpdateFeedback;

public class UpdateFeedbackCommand : IRequest<APIResponse>
{
    public string UserId { get; set; }
    public string FeedbackId { get; set; }  
    public UpdateFeedbackRequest Feedback { get; set; }

    public UpdateFeedbackCommand(string userId, string feedbackId, UpdateFeedbackRequest feedback)
    {
        UserId = userId;
        Feedback = feedback;
        FeedbackId = feedbackId;
    }
}
