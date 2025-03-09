using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public async Task<PagedList<Tag>> GetTagsWithFilters(string? Id, string? Name, int? page, int? eachPage)
        {
            var query = _context.Tags.AsQueryable();

            if (!string.IsNullOrEmpty(Id))
            {
                query = query.Where(c => c.Id == Id);
            }
            if (!string.IsNullOrEmpty(Name))
            {
                query = query.Where(c => c.Name.Contains(Name));
            }

            return new PagedList<Tag>(await query.ToListAsync(), query.Count(), (int)page!, (int)eachPage!);
        }
    }
}
