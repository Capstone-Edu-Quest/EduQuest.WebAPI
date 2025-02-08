using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Domain.Repository
{
	public interface ICourseRepository : IGenericRepository<Course>
	{
		Task<Course> GetCourseById(string Id);
		Task<IEnumerable<Course>> GetCourseByUserId(string Id);
		//Task<IEnumerable<Course>> GetAllCourse();
		Task<int> GetTotalTime(string courseId);
		Task<bool> IsExist(string courseId);
    }
}
