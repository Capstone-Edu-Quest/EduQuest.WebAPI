using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.FavoriteCourse.Query.SearchFavoriteCourse
{
	public class SearchFavoriteCourseQuery : IRequest<APIResponse>
	{
		public int PageNo { get; set; }
		public int EachPage { get; set; }
        public string? Name { get; set; }
        public string UserId { get; set; }

		public SearchFavoriteCourseQuery(int pageNo, int eachPage, string? name, string userId)
		{
			PageNo = pageNo;
			EachPage = eachPage;
			Name = name;
			UserId = userId;
		}
	}
}
