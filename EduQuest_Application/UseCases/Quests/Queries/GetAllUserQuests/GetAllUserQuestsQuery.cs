using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Quests.Queries.GetAllUserQuests;

public class GetAllUserQuestsQuery : IRequest<APIResponse>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? PointToComplete { get; set; }
    public int? Type { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string UserId { get; set; }
    public int Page { get; set; }
    public int EachPage { get; set; }

    public GetAllUserQuestsQuery(string? title, string? description, int? pointToComplete, int? type, 
        DateTime? startDate, DateTime? dueDate, string userId, int page, int eachPage)
    {
        Title = title;
        Description = description;
        PointToComplete = pointToComplete;
        Type = type;
        StartDate = startDate;
        DueDate = dueDate;
        UserId = userId;
        Page = page;
        EachPage = eachPage;
    }
}
