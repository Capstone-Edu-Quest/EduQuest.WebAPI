using EduQuest_Application.DTO.Request.Quests;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Ocsp;

namespace EduQuest_Infrastructure.Repository;

public class UserQuestRepository : GenericRepository<UserQuest>, IUserQuestRepository
{
    private readonly ApplicationDbContext _context;

    public UserQuestRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }


    public async Task<bool> AddNewQuestToAllUseQuest(Quest newQuest)
    {
        List<string> UserIds = new List<string>();
        UserIds = await _context.Users.Select(u => u.Id).ToListAsync();
        List<UserQuest> userQuests = new List<UserQuest>();
        List<QuestReward> rewards = (List<QuestReward>)newQuest.Rewards;
        foreach (string UserId in UserIds)
        {
            // Tạo UserQuest mới
            UserQuest temp = new UserQuest
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now.ToUniversalTime(),
                Title = newQuest.Title,
                Type = newQuest.Type,
                IsDaily = newQuest.IsDaily,
                Description = newQuest.Description,
                StartDate = DateTime.Now.ToUniversalTime(),
                DueDate = DateTime.Now.AddMinutes(int.Parse(newQuest.TimeToComplete!)).ToUniversalTime(),
                PointToComplete = newQuest.PointToComplete!,
                CurrentPoint = 0,
                IsCompleted = false,
                UserId = UserId,
                Rewards = new List<QuestReward>()
            };

            //add reward
            foreach (var reward in rewards)
            {
                temp.Rewards.Add(new QuestReward
                {
                    Id = Guid.NewGuid().ToString(),
                    RewardType = reward.RewardType,
                    RewardValue = reward.RewardValue,
                    QuestId = null,
                    UserQuestId = temp.Id
                });
            }

            userQuests.Add(temp);
        }
        await _context.UserQuests.AddRangeAsync(userQuests);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }
}
