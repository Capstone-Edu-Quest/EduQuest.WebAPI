using AutoMapper;
using EduQuest_Application.DTO.Response;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System.Linq;

namespace EduQuest_Application.UseCases.Courses.Queries.SearchCourse
{
	public class SearchCourseQueryHandler : IRequestHandler<SearchCourseQuery, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;

		public SearchCourseQueryHandler(ICourseRepository courseRepository, IUserRepository userRepository, IMapper mapper)
		{
			_courseRepository = courseRepository;
			_userRepository = userRepository;
			_mapper = mapper;
		}

		public async Task<APIResponse> Handle(SearchCourseQuery request, CancellationToken cancellationToken)
		{
			var listCourse = await _courseRepository.GetAll();
			if(request.SearchRequest.KeywordName != null)
			{
				listCourse = listCourse.Where(x => SearchHelper.ConvertVietnameseToEnglish(x.Title.ToLower()).Contains(SearchHelper.ConvertVietnameseToEnglish(request.SearchRequest.KeywordName.ToLower())));
			} else if(request.SearchRequest.DateTo != null)
			{
				listCourse = listCourse.Where(x => x.LastUpdated <= request.SearchRequest.DateTo);
			}
			else if (request.SearchRequest.DateFrom != null)
			{
				listCourse = listCourse.Where(x => x.LastUpdated >= request.SearchRequest.DateFrom);
			}
			else if (request.SearchRequest.DateFrom != null && request.SearchRequest.DateTo != null)
			{
				listCourse = listCourse.Where(x => x.LastUpdated >= request.SearchRequest.DateFrom && x.LastUpdated <= request.SearchRequest.DateTo);
			}
			else if (request.SearchRequest.Author != null)
			{
				listCourse = listCourse.Where(x => SearchHelper.ConvertVietnameseToEnglish(x.CreatedBy.ToLower()).Contains(SearchHelper.ConvertVietnameseToEnglish(request.SearchRequest.Author)));
			}

			//Chưa check rating

			var listCourseResponse = _mapper.Map<List<CourseSearchResponse>>(listCourse); //Chưa check Discount Price, chưa map Rating
			foreach (var course in listCourseResponse)
			{
				var user = await _userRepository.GetById(course.CreatedBy); 
				course.Author = user!.Username; 
			}

			int totalItem = listCourseResponse.Count;
			var result = listCourseResponse.Skip((request.PageNo - 1) * request.EachPage)
											.Take(request.EachPage)
											.ToList();
			return new APIResponse
			{
				IsError = false,
				Payload = result,
				Errors = null,
			};


		}
	}
}
