using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Repository
{
	public interface IMaterialRepository : IGenericRepository<Material>
	{
		Task<List<Material>> GetMaterialsByIds(List<string> materialIds);
		Task<Material> GetMataterialQuizAssById(string materialId);
		Task<List<Material>> GetByUserId(string userId);
		Task<Material> GetMaterialWithLesson(string materialId);
		Task<bool> IsOwnerThisMaterial(string userId, string materialId);
		Task<List<Material>> GetByUserIdAsync(string userId);
		Task<Material> GetMaterialIncludeQuizAssignment(string materialId);
		Task<List<Material>> GetMaterialsByType(List<string> materialIds, string type);
    }
}
