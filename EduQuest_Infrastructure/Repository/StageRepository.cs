using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Infrastructure.Repository
{
	public class StageRepository : GenericRepository<Stage>, IStageRepository
	{
		private readonly ApplicationDbContext _context;

		public StageRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}
	}
}
