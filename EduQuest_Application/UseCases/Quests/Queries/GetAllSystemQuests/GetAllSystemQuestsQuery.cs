using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Quests.Queries.GetAllSystemQuests;

public class GetAllSystemQuestsQuery : IRequest<APIResponse>
{
    public string? Title { get; set; }
    public int? QuestType { get; set; }
    public int? Type { get; set; }
    public int? QuestValue { get; set; }
    public string UserId { get; set; }
    public int Page {  get; set; }
    public int EachPage { get; set; }

    public GetAllSystemQuestsQuery(string? title, int? questType, int? type, int? questValue, 
        string userId, int page, int eachPage)
    {
        Title = title;
        QuestType = questType;
        Type = type;
        QuestValue = questValue;
        UserId = userId;
        Page = page;
        EachPage = eachPage;
    }
}
