using EduQuest_Domain.Models.Response;
using MediatR;

namespace EduQuest_Application.UseCases.Expert.Commands.ApproveCourse;

public class ApproveCourseCommand : IRequest<APIResponse>
{
    public string CourseId { get; set; } = null!;
    public bool isApprove { get; set; }
    public string? RejectedReason { get; set; }
}

