
using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Infrastructure.Repository;

public class UserQuestRepository : GenericRepository<UserQuest>, IUserQuestRepository
{
    private readonly ApplicationDbContext _context;

    public UserQuestRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }


    public async Task<bool> AddNewQuestToAllUserQuest(Quest newQuest)
    {
        string roleId = ((int)UserRole.Learner).ToString();
        List<string> UserIds = new List<string>();
        UserIds = await _context.Users.Where(u => u.RoleId == roleId).Select(u => u.Id).ToListAsync();
        List<UserQuest> userQuests = new List<UserQuest>();
        ICollection<QuestReward> rewards = newQuest.Rewards;
        foreach (string UserId in UserIds)
        {
            // new UserQuest
            UserQuest temp = new UserQuest
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now.ToUniversalTime(),
                Title = newQuest.Title,
                Type = newQuest.Type,
                IsDaily = newQuest.IsDaily,
                Description = newQuest.Description,
                StartDate = DateTime.Now.ToUniversalTime(),
                DueDate = newQuest.TimeToComplete > 0 ? DateTime.Now.AddMinutes((double)newQuest.TimeToComplete!).ToUniversalTime() : null,
                PointToComplete = newQuest.PointToComplete!,
                CurrentPoint = 0,
                IsCompleted = false,
                UserId = UserId,
                QuestId = newQuest.Id,
                Rewards = new List<UserQuestQuestReward>()
            };

            //add reward
            foreach (var reward in rewards)
            {
                temp.Rewards.Add(new UserQuestQuestReward
                {
                    UserQuestId = temp.Id,
                    QuestRewardId = reward.Id,
                });
            }

            userQuests.Add(temp);
        }

        await _context.UserQuests.AddRangeAsync(userQuests);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> UpdateAllUserQuest(Quest updatedQuest)
    {
        string roleId = ((int)UserRole.Learner).ToString();
        List<string> UserIds = new List<string>();
        UserIds = await _context.Users.Where(u => u.RoleId == roleId).Select(u => u.Id).ToListAsync();
        List<UserQuest> userQuests = await _context.UserQuests.Where(ur => ur.QuestId == updatedQuest.Id).ToListAsync();
        ICollection<QuestReward> rewards = updatedQuest.Rewards;

        foreach(var userQuest in  userQuests)
        {
            userQuest.Title = updatedQuest.Title;
            userQuest.Description = updatedQuest.Description;
            userQuest.Type = updatedQuest.Type;
            userQuest.PointToComplete = updatedQuest.PointToComplete;
            userQuest.DueDate = updatedQuest.TimeToComplete > 0 ? userQuest.StartDate!.Value.AddMinutes((double)updatedQuest.TimeToComplete!).ToUniversalTime() : null;
            userQuest.IsDaily = updatedQuest.IsDaily;
        }
        _context.UserQuests.UpdateRange(userQuests);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }
}
