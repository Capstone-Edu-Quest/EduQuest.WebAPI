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
    public int? QuestType { get; set; }
    public int? Type { get; set; }
    public int? PointToComplete { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public bool? IsComplete { get; set; }
    public string UserId { get; set; }
    public int Page { get; set; }
    public int EachPage { get; set; }

    public GetAllUserQuestsQuery(string? title, int? questType, int? type, int? pointToComplete, 
        DateTime? startDate, DateTime? dueDate, bool? isComplete, string userId, int page, int eachPage)
    {
        Title = title;
        QuestType = questType;
        Type = type;
        PointToComplete = pointToComplete;
        StartDate = startDate;
        DueDate = dueDate;
        IsComplete = isComplete;
        UserId = userId;
        Page = page;
        EachPage = eachPage;
    }
}
