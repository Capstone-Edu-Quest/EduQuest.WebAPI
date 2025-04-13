

namespace EduQuest_Application.ExternalServices.QuartzService;

public interface IQuartzService
{
    Task AddNewQuestToAllUser(string questId);
    Task AddAllQuestsToNewUser(string userId);
    Task UpdateAllUserQuest(string questId);
    Task UpdateUserPackageAccountType(string userId);
    Task TransferToInstructor(string transactionId);
    /*Task ResetQuestProgress();
    Task ResetDailyQuests();*/
}
