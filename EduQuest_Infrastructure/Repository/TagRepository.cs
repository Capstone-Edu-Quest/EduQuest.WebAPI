using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Infrastructure.Repository
{
	public class TagRepository : GenericRepository<Tag>, ITagRepository
	{
		private readonly ApplicationDbContext _context;

		public TagRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

        public async Task<Tag> GetTagByName(string name)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name.ToUpper().Trim().Equals(name.ToUpper().Trim()));
            if (tag != null)
            {
                return tag;
            }
            return null;
        }


        public async Task<PagedList<Tag>> GetTagsWithFilters(string? Id, string? Name, int page, int eachPage, int? Type)
        {
            var query = _context.Tags.AsQueryable();

            if (!string.IsNullOrEmpty(Id))
            {
                query = query.Where(c => c.Id == Id);
            }
			if (Type.HasValue)
			{
				var typeName = Enum.GetName(typeof(TagType), Type.Value);
				if (!string.IsNullOrEmpty(typeName))
				{
					query = query.Where(c => c.Type == typeName);
				}
			}

			var tags = await query.ToListAsync();
			if (!string.IsNullOrEmpty(Name))
			{
				var name = ContentHelper.ConvertVietnameseToEnglish(Name?.ToLower());
				tags = tags
				.Where(x =>
						(string.IsNullOrWhiteSpace(Name) || ContentHelper.ConvertVietnameseToEnglish(x.Name.ToLower()).Contains(name))
					)
					.ToList();
			}
			var totalItems = tags.Count;
			var pagedTags = tags
				.Skip((page - 1) * eachPage)
				.Take(eachPage)
				.ToList();

			return new PagedList<Tag>(pagedTags, totalItems, page, eachPage);

			//return await query.Pagination(page, eachPage).ToPagedListAsync(page, eachPage);
			//return new PagedList<Tag>(await query.ToListAsync(), query.Count(), (int)page!, (int)eachPage!);
		}
    }
}
