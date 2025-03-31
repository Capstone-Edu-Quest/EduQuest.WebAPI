

namespace EduQuest_Application.ExternalServices.QuartzService;

public interface IQuartzService
{
    Task AddNewQuestToAllUser(string questId);
    Task AddAllQuestsToNewUser(string userId);
    Task UpdateAllUserQuest(string questId);
    Task UpdateUserPackageAccountType(string userId);
    /*Task ResetQuestProgress();
    Task ResetDailyQuests();*/
}
