using EduQuest_Application.DTO.Request.Courses;
using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Courses.Command.ReviewAssignment;

public class ReviewAssignmentCommand : IRequest<APIResponse>
{
    public string UserId { get; set; }
    public GradingAssignmentDto grade {  get; set; }

    public ReviewAssignmentCommand(string userId, GradingAssignmentDto grade)
    {
        UserId = userId;
        this.grade = grade;
    }
}
