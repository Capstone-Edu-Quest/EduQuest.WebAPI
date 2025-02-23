using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Repository
{
	public interface IStageRepository : IGenericRepository<Stage>
	{
		Task<List<Stage>> GetByCourseId(string id);
		Task<int?> GetMaxLevelInThisCourse(string id);
		Task<bool> DeleteStagesByCourseId(string courseId);	
	}
}
