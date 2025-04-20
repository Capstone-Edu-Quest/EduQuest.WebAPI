
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Extensions;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using Nest;
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
        return int.Parse(temp[0]);
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
            return double.Parse(temp[1]);
        }

        return null;
    }
    private double? GetTimeToComplete(string input)
    {
        string[] temp = input.Split(",");
        if(temp.Length > 1)
        {
            return double.Parse(temp[1]);
        }
        return null;
    }

    public async Task<bool> AddNewQuestToAllUserQuest(Quest newQuest)
    {
        string roleId = ((int)UserRole.Learner).ToString();
        List<string> UserIds = new List<string>();
        UserIds = await _context.Users.Where(u => u.RoleId == roleId).Select(u => u.Id).ToListAsync();
        List<UserQuest> userQuests = new List<UserQuest>();
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
                RewardTypes = newQuest.RewardTypes,
                RewardValues = newQuest.RewardValues,
                StartDate = DateTime.Now.ToUniversalTime(),
                DueDate = GetTimeToComplete(newQuest) != null ? DateTime.Now.AddMinutes(GetTimeToComplete(newQuest)!.Value).ToUniversalTime() : null,
                PointToComplete = GetPointToComplete(newQuest),
                CurrentPoint = 0,
                IsCompleted = false,
                UserId = UserId,
                QuestId = newQuest.Id,
            };
            userQuests.Add(temp);
        }

        await _context.UserQuests.AddRangeAsync(userQuests);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> AddQuestsToNewUser(string userId)
    {
        var quests = await _context.Quests.ToListAsync();
        DateTime now = DateTime.Now.ToUniversalTime();
        DateTime StarDate;
        if (now.TimeOfDay < new TimeSpan(5, 0, 0))
        {
            StarDate = new DateTime(now.Year, now.Month, now.Day, 5, 0, 0).AddDays(-1);
        }
        else
        {
            StarDate = new DateTime(now.Year, now.Month, now.Day, 5, 0, 0);
        }
        //List<UserQuest> userQuests = new List<UserQuest>();
        var userQuests = quests.Select(quest => new UserQuest
        {
            Id = Guid.NewGuid().ToString(),
            CreatedAt = now,
            Title = quest.Title,
            Type = quest.Type,
            QuestType = quest.QuestType,
            QuestValues = quest.QuestValues,
            RewardTypes = quest.RewardTypes,
            RewardValues = quest.RewardValues,
            StartDate = StarDate.ToUniversalTime(),
            DueDate = GetTimeToComplete(quest) != null
            ? StarDate.AddMinutes(GetTimeToComplete(quest).Value).ToUniversalTime()
            : null,
            PointToComplete = GetPointToComplete(quest),
            CurrentPoint = 0,
            IsCompleted = false,
            UserId = userId,
            QuestId = quest.Id,
        }).ToList();
        await _context.UserQuests.AddRangeAsync(userQuests);
        int result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> UpdateAllUserQuest(Quest updatedQuest)
    {
        /*List<UserQuest> userQuests = await _context.UserQuests.Where(ur => ur.QuestId == updatedQuest.Id).ToListAsync();

        foreach(var userQuest in  userQuests)
        {
            DateTime temp = userQuest.StartDate!.Value;
            userQuest.Title = updatedQuest.Title;
            userQuest.Type = updatedQuest.Type;
            userQuest.QuestType = updatedQuest.QuestType;
            userQuest.PointToComplete = GetPointToComplete(updatedQuest);
            userQuest.DueDate = GetTimeToComplete(updatedQuest) != null ? temp.AddMinutes(GetTimeToComplete(updatedQuest)!.Value).ToUniversalTime() : null;
            userQuest.QuestValues = updatedQuest.QuestValues;
            userQuest.RewardValues = updatedQuest.RewardValues;
            userQuest.RewardTypes = updatedQuest.RewardTypes;
            userQuest.QuestValues = updatedQuest.QuestValues;
        }
        _context.UserQuests.UpdateRange(userQuests);
        int result = await _context.SaveChangesAsync();
        return result > 0;*/
        int affectedRow = await _context.UserQuests.Where(ur => ur.QuestId == updatedQuest.Id)
            .ExecuteUpdateAsync(q => q
            .SetProperty(uq => uq.Title, updatedQuest.Title)
            .SetProperty(uq => uq.Type, updatedQuest.Type)
            .SetProperty(uq => uq.QuestType, updatedQuest.QuestType)
            .SetProperty(uq => uq.PointToComplete, GetPointToComplete(updatedQuest))
            .SetProperty(uq => uq.QuestValues, updatedQuest.QuestValues)
            .SetProperty(uq => uq.RewardValues, updatedQuest.RewardValues)
            .SetProperty(uq => uq.RewardTypes, updatedQuest.RewardTypes)
            .SetProperty(uq => uq.DueDate, uq => GetTimeToComplete(updatedQuest.QuestValues!) != null ? 
            uq.StartDate!.Value.AddMinutes(GetTimeToComplete(uq.QuestValues!)!.Value).ToUniversalTime() : null)
            );
        return affectedRow > 0;
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
    public async Task<bool> UpdateUserQuestsProgress(string userId, QuestType questType, double addedPoint)
    {

        /*var userQuests = await _context.UserQuests
        .Where(uq => uq.UserId == userId && uq.Type == (int)questType && uq.IsCompleted == false)
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

        return await _context.SaveChangesAsync() > 0;*/
        int affectedRows = await _context.UserQuests
        .Where(uq => uq.UserId == userId && uq.Type == (int)questType && uq.IsCompleted == false)
        .ExecuteUpdateAsync(q => q
            .SetProperty(uq => uq.CurrentPoint, uq => uq.CurrentPoint + addedPoint)
            .SetProperty(uq => uq.IsCompleted, uq => uq.CurrentPoint + addedPoint >= uq.PointToComplete));

        return affectedRows > 0;
    }
    
    public async Task<bool> ResetQuestProgress()
    {
        var quests = await _context.UserQuests
            .Where(q => q.DueDate.HasValue && q.DueDate <= DateTime.Now.ToUniversalTime() && q.IsCompleted == false && q.Type == (int)ResetType.OneTime)
            .ToListAsync();

        foreach (var quest in quests)
        {
            quest.CurrentPoint = 0;
            quest.StartDate = DateTime.Now.ToUniversalTime();
            quest.DueDate = DateTime.Now.AddMinutes(GetTimeToComplete(quest.QuestValues!).Value).ToUniversalTime();

        }

        return await _context.SaveChangesAsync() > 0;
        /*int affectedRows = await _context.UserQuests
        .Where(q => q.DueDate.HasValue && q.DueDate <= DateTime.Now.ToUniversalTime() && q.QuestType == (int)ResetType.OneTime && q.IsCompleted == false)
        .ExecuteUpdateAsync(q => q
        .SetProperty(uq => uq.PointToComplete, 0)
        .SetProperty(uq => uq.StartDate, DateTime.Now.ToUniversalTime())
        .SetProperty(uq => uq.DueDate, uq => uq.StartDate!.Value.
            AddMinutes(GetTimeToComplete(uq.QuestValues!)!.Value).ToUniversalTime())
        );

        return affectedRows > 0;*/
    }
    public async Task<bool> ResetDailyQuests()
    {
        int affectedRows = await _context.UserQuests
        .Where(q => q.Type == (int)ResetType.Daily)
        .ExecuteUpdateAsync(q => q
            .SetProperty(uq => uq.CurrentPoint, 0)
            .SetProperty(uq => uq.IsCompleted, false)
            .SetProperty(uq => uq.IsRewardClaimed, false)
            .SetProperty(uq => uq.StartDate, DateTime.Now.ToUniversalTime())
            .SetProperty(uq => uq.DueDate, DateTime.Now.AddDays(1).ToUniversalTime()));

        return affectedRows > 0;
    }
}
