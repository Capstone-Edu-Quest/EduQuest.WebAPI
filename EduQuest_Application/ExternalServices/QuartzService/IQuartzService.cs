

namespace EduQuest_Application.ExternalServices.QuartzService;

public interface IQuartzService
{
    Task AddNewQuestToAllUser(string questId);
    Task UpdateAllUserQuest(string questId);
}
