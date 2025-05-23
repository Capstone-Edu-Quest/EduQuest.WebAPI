﻿using EduQuest_Application.DTO.Response.UserStatistics;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using Nest;
using Stripe;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Infrastructure.Repository;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<(List<User> Users, int TotalCount)> SearchUsersAsync(
    string? username,
    string? email,
    string? phone,
    string? status,
    string? roleId,
    int pageNo,
    int eachPage)
    {
        // Start with all users
        IQueryable<User> query = _context.Users.AsNoTracking();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(username))
        {
            query = query.Where(u => u.Username.Contains(username));
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            query = query.Where(u => u.Email.Contains(email));
        }

        if (!string.IsNullOrWhiteSpace(phone))
        {
            query = query.Where(u => u.Phone.Contains(phone));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(u => u.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(roleId))
        {
            query = query.Where(u => u.RoleId == roleId);
        }

        int totalCount = await query.CountAsync();

        var users = await query
            .Skip(pageNo * eachPage)
            .Take(eachPage)
            .ToListAsync();

        return (users, totalCount);
    }


    public async Task<List<User>> GetUserByAssignToExpet(string expertId)
    {
        return await _context.Users.AsNoTracking().Where(x => x.AssignToExpertId.Equals(expertId) && x.Status.ToLower() == "pending").ToListAsync();
    }
    public async Task<List<User>> GetUserByStatus(string status)
    {
        var result = _context.Users.AsQueryable().AsNoTracking();

        if (!string.IsNullOrEmpty(status))
        {
            result = result.Where(x => x.Status!.ToLower().Equals(status.ToLower()));
        }

        var finalResult = await result.ToListAsync();
        return finalResult;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .Include(a => a.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email!.ToLower().Equals(email.ToLower()));
    }

    public async Task<List<User>?> GetByUserIds(List<string> ids)
    {

        var result = await _context.Users
            .Where(c => ids.Contains(c.Id))
            .ToListAsync();
        return result;
    }

    public async Task<User> GetUserById(string userId)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<bool> UpdateUserPackageAccountType(string userId)
    {
        int affectedRow = await _context.Users.Where(u => u.Id == userId).ExecuteUpdateAsync(u => u.SetProperty(u => u.Package, PackageEnum.Pro.ToString()));
        return affectedRow > 0;
    }
    public async Task<AdminDasboardUsers> GetAdminDashBoardStatistic()
    {
        AdminDasboardUsers result = new AdminDasboardUsers();

        var Month = DateTime.Now.Month;
        var Year = DateTime.Now.Year;   
        var TotalUsers = _context.Users.Include(u => u.UserMeta).AsQueryable();

        result.TotalUsers = await TotalUsers.CountAsync();

        result.MonthlyActiveUsers = await TotalUsers.
            Where(t => t.UserMeta!.LastActive.Month == Month && t.UserMeta.LastActive.Year == Year).CountAsync();
       
        result.ThisMonthNewUsers = await TotalUsers.
            Where(t => t.CreatedAt!.Value.Month == Month && t.CreatedAt.Value.Year == Year).CountAsync();
       
        result.GraphData = await CreateGraphDataUser(TotalUsers);
        
        return result;
    }
    /*private async Task<List<UserGraphData>> CreateGraphDataUser(IQueryable<User>? TotalUsers)
    {
        DateTime end = DateTime.Now;
        DateTime start = end.AddMonths(-5);
        var GraphUser = await TotalUsers
            .Where(t => t.CreatedAt >= start.ToUniversalTime() && t.CreatedAt <= end.ToUniversalTime())
            .GroupBy(user => new { user.CreatedAt.Value.Year, user.CreatedAt.Value.Month })
            .Select(g => new UserGraphData
            {
                TotalUser = g.Count(),
                TotalActiveUser = g.Count(user => user.Status == AccountStatus.Active.ToString()),
                TotalProUser = g.Count(user => user.Package == PackageEnum.Pro.ToString()),
                Date = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy") // Định dạng YYYY-MM
            })
        .ToListAsync();

        return GraphUser;
    }*/
    private async Task<List<UserGraphData>> CreateGraphDataUser(IQueryable<User>? TotalUsers)
    {
        DateTime tempDate = DateTime.Now.AddMonths(-6);
        DateTime start = new DateTime(tempDate.Year, tempDate.Month, 1);
        List<UserGraphData> result = new List<UserGraphData>();
        var users = await TotalUsers.ToListAsync();

        for (int i = 0; i <= 5; i++)
        {
            var currentMonth = tempDate.AddMonths(i);
            start = start.AddMonths(1);
            UserGraphData temp = new UserGraphData
            {
                Date = currentMonth.ToString("MMM yyyy"),
                TotalUser = users.Count(u => u.CreatedAt.Value < start.ToUniversalTime()),
                TotalActiveUser = users.Count(u => u.CreatedAt.Value < start.ToUniversalTime() && u.Status == AccountStatus.Active.ToString()),
                TotalProUser = users.Count(u => u.CreatedAt.Value < start.ToUniversalTime() && u.Package == PackageEnum.Pro.ToString())
            };

            result.Add(temp);
        }
        return result;
    }

    //public async Task<List<User>> GetByRoleId(string roleId, string? tagId)
    //{
    //    return await _context.Users
    //        .Where(x => x.RoleId == roleId && (tagId == null || x.ExpertiseTagId==tagId))
    //        .ToListAsync();
    //}

    public async Task<List<User>> GetByRoleId(string roleId)
    {
        return await _context.Users
            .Where(x => x.RoleId == roleId)
            .ToListAsync();
    }
}
