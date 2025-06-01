using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Infrastructure.Repository
{
	public class AssignmentRepository : GenericRepository<Assignment>, IAssignmentRepository
	{
		private readonly ApplicationDbContext _context;

		public AssignmentRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<Assignment>> GetByUserId(string userId, SearchLessonContent? info)
		{
			var query =  _context.Assignments.Where(x => x.UserId == userId);
			if(info != null)
			{
				if (info.TagType != null)
				{
					var type = Enum.GetName(typeof(TagType), info.TagType);
					query = query.Where(x => x.Tags.Any(tag => tag.Type == type));
				}

				if (info.TagIds != null && info.TagIds.Any())
				{
					var tagList = info.TagIds;
					query = query.Where(x => x.Tags.Any(tag => tagList.Contains(tag.Id)));
				}
			}
			return await query.ToListAsync();
		}
	}
}
