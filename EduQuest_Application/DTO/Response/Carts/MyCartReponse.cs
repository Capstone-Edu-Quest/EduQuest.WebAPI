using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response.Carts
{
	public class MyCartReponse : IMapFrom<Cart>, IMapTo<Cart>
	{
        public string Id { get; set; }
        public decimal? Total { get; set; }
        public List<CourseSearchResponse>? Courses { get; set; }
    }
}
