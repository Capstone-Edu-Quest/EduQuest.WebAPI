﻿using EduQuest_Application.DTO.Request.Materials;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Infrastructure.Repository
{
	public class MaterialRepository : GenericRepository<Material>, IMaterialRepository
	{
		private readonly ApplicationDbContext _context;

		public MaterialRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<List<Material>> GetByUserId(string userId, SearchLessonContent? info)
		{
			var query =  _context.Materials.Where(x => x.UserId == userId);
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

		public async Task<Material> GetMataterialQuizAssById(string materialId)
		{
			return await _context.Materials.FirstOrDefaultAsync(x => x.Id == materialId);
		}

		public async Task<List<Material>> GetMaterialsByIds(List<string> materialIds)
		{
			return await _context.Materials.Where(m => materialIds.Contains(m.Id)).ToListAsync();
		}

		

		public async Task<bool> IsOwnerThisMaterial(string userId, string materialId)
		{
			var material = await _context.Materials.FirstOrDefaultAsync(x => x.Id == materialId && x.UserId == userId);
			return material != null;
		}

        public async Task<List<Material>> GetByUserIdAsync(string userId)
        {
            return await _context.Materials.AsNoTracking().Where(x => x.UserId == userId).ToListAsync();
        }

		public async Task<Material> GetMaterialIncludeQuizAssignment(string materialId)
		{
			return await _context.Materials.FirstOrDefaultAsync(x => x.Id == materialId);
		}

		public async Task<List<Material>> GetMaterialsByType(List<string> materialIds, string type)
		{
			return await _context.Materials.Where(x => materialIds.Contains(x.Id) && x.Type == type).ToListAsync();
		}

    }
}
