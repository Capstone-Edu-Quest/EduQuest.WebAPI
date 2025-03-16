
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Extensions;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static EduQuest_Domain.Enums.GeneralEnums;
using static EduQuest_Domain.Enums.QuestEnum;

namespace EduQuest_Infrastructure.Repository;

public class UserQuestRepository : GenericRepository<UserQuest>, IUserQuestRepository
{
    private readonly ApplicationDbContext _context;

    public UserQuestRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    private int GetPointToComplete(Quest newQuest)
    {
        string[] temp = newQuest.QuestValues!.Split(",");
        return int.Parse(temp[1]);
    }
    private double? GetTimeToComplete(Quest newQuest)
    {
        if (newQuest.QuestType == (int)QuestType.STAGE_TIME ||
            newQuest.QuestType == (int)QuestType.MATERIAL_TIME ||
            newQuest.QuestType == (int)QuestType.QUIZ_TIME ||
            newQuest.QuestType == (int)QuestType.COURSE_TIME ||
            newQuest.QuestType == (int)QuestType.LEARNING_TIME_TIME)
        {
            string[] temp = newQuest.QuestValues!.Split(",");
            return double.Parse(temp[2]);
        }

        return null;
    }
    public async Task<bool> AddNewQuestToAllUserQuest(Quest newQuest)
    {
        string roleId = ((int)UserRole.Learner).ToString();
        List<string> UserIds = new List<string>();
        UserIds = await _context.Users.Where(u => u.RoleId == roleId).Select(u => u.Id).ToListAsync();
        List<UserQuest> userQuests = new List<UserQuest>();
        ICollection<Reward> rewards = newQuest.Rewards;
        foreach (string UserId in UserIds)
        {
            // new UserQuest
            UserQuest temp = new UserQuest
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now.ToUniversalTime(),
                Title = newQuest.Title,
                Type = newQuest.Type,
                QuestType = newQuest.QuestType,
                QuestValues = newQuest.QuestValues,
                StartDate = DateTime.Now.ToUniversalTime(),
                DueDate = GetTimeToComplete(newQuest) != null ? DateTime.Now.AddMinutes(GetTimeToComplete(newQuest)!.Value).ToUniversalTime() : null,
                PointToComplete = GetPointToComplete(newQuest),
                CurrentPoint = 0,
                IsCompleted = false,
                UserId = UserId,
                QuestId = newQuest.Id,
                //Rewards = new List<UserQuestReward>()
            };

            //add reward
            //foreach (var reward in rewards)
            //{
            //    temp.Rewards.Add(new UserQuestReward
            //    {
            //        UserQuestId = temp.Id,
            //        QuestRewardId = reward.Id,
            //    });
            //}

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
        ICollection<Reward> rewards = updatedQuest.Rewards;

        foreach(var userQuest in  userQuests)
        {
            userQuest.Title = updatedQuest.Title;
            userQuest.Type = updatedQuest.Type;
            userQuest.QuestType = updatedQuest.QuestType;
            userQuest.PointToComplete = GetPointToComplete(updatedQuest);
            userQuest.DueDate = GetTimeToComplete(updatedQuest) != null ? DateTime.Now.AddMinutes(GetTimeToComplete(updatedQuest)!.Value).ToUniversalTime() : null;
            userQuest.QuestValues = updatedQuest.QuestValues;
        }
        _context.UserQuests.UpdateRange(userQuests);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }


    public async Task<PagedList<UserQuest>> GetAllUserQuests(string? title, int? questType, int? type, int? pointToComplete,
        DateTime? startDate, DateTime? dueDate, bool? isComplete, string userId, int page, int eachPage)
    {
        var result = _context.UserQuests
        //.Include(q => q.Rewards)
        .Where(q => q.UserId == userId)
        .AsQueryable();

        #region Filter
        if (!string.IsNullOrEmpty(title))
        {
            result = from r in result
                     where r.Title!.Contains(title)
                     select r;
        }
        if (questType.HasValue)
        {
            result = from r in result
                     where r.QuestType! == questType.Value
                     select r;
        }
        if (type.HasValue)
        {
            result = from r in result
                     where r.Type! >= type.Value
                     select r;
        }
        if(startDate.HasValue)
        {
            result = from r in result
                     where r.StartDate! >= startDate.Value
                     select r;
        }
        if (dueDate.HasValue)
        {
            result = from r in result
                     where r.DueDate >= dueDate.Value
                     select r;
        }
        if (pointToComplete.HasValue)
        {
            result = from r in result
                     where r.PointToComplete! >= pointToComplete.Value
                     select r;
        }
        if (isComplete.HasValue)
        {
            result = from r in result
                     where r.IsCompleted == isComplete.Value
                     select r;
        }
        #endregion

        var response = await result.Pagination(page, eachPage).ToPagedListAsync(page, eachPage);
        return response;
    }
    public async Task<List<Reward>> GetUserQuestRewardAsync(List<string> rewardIds)
    {
        if (rewardIds == null || !rewardIds.Any())
        {
            return new List<Reward>();
        }

        var result = await _context.QuestRewards
            .Where(q => rewardIds.Contains(q.Id))
            .ToListAsync();

        return result;
    }

    public async Task<bool> UpdateUserQuestsProgress(string userId, QuestType questType, int addedPoint)
    {
        var userQuests = await _context.UserQuests
        .Where(uq => uq.UserId == userId && uq.Type == (int)questType)
        .ToListAsync();

        foreach (var userQuest in userQuests)
        {
            userQuest.CurrentPoint += addedPoint;
            if (userQuest.CurrentPoint >= userQuest.PointToComplete)
            {
                userQuest.IsCompleted = true;
                userQuest.CurrentPoint = userQuest.PointToComplete;
            }
        }

        return await _context.SaveChangesAsync() > 0;
    }
}
