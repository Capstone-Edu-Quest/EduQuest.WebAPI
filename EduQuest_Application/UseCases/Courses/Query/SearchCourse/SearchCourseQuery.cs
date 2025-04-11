using EduQuest_Application.DTO.Request.Courses;
using EduQuest_Domain.Models.Request;
using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.Courses.Queries.SearchCourse
{
    public class SearchCourseQuery : IRequest<APIResponse>
	{
		public int PageNo { get; set; }
		public int EachPage { get; set; }
        public string UserId { get; set; }
        public SearchCourseRequestDto? SearchRequest { get; set; }

		public SearchCourseQuery(int pageNo, int eachPage, string userId, SearchCourseRequestDto? searchRequest)
		{
			PageNo = pageNo;
			EachPage = eachPage;
			UserId = userId;
			SearchRequest = searchRequest;
		}
	}
}
