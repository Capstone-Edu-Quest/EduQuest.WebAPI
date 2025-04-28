

namespace EduQuest_Application.ExternalServices.QuartzService;

public interface IQuartzService
{
    Task AddNewQuestToAllUser(string questId);
    Task AddAllQuestsToNewUser(string userId);
    Task UpdateAllUserQuest(string questId);
    Task UpdateUserPackageAccountMonthly(string userId);
	Task UpdateUserPackageAccountYearLy(string userId);
	Task TransferToInstructor(string TransactionId);
    /*Task ResetQuestProgress();
    Task ResetDailyQuests();*/
}
