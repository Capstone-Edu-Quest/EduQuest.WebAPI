using AutoMapper;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.DTO.Response;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.Courses.Queries.SearchCourse
{
	public class SearchCourseQueryHandler : IRequestHandler<SearchCourseQuery, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;
		private readonly ICourseStatisticRepository _courseStatisticRepository;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;
		private readonly IRedisCaching _redisCaching;

		public SearchCourseQueryHandler(ICourseRepository courseRepository, 
			ICourseStatisticRepository courseStatisticRepository, 
			IUserRepository userRepository, 
			IMapper mapper, 
			IRedisCaching redisCaching)
		{
			_courseRepository = courseRepository;
			_courseStatisticRepository = courseStatisticRepository;
			_userRepository = userRepository;
			_mapper = mapper;
			_redisCaching = redisCaching;
		}

		public async Task<APIResponse> Handle(SearchCourseQuery request, CancellationToken cancellationToken)
		{
			//var cacheKey = $"searchCourse_{request.UserId}_{request.SearchRequest!.KeywordName}";
			//var cacheSearchKey = $"searchHistory_{request.UserId}_{request.SearchRequest!.KeywordName}";

			//var cachedDataString = await _redisCaching.GetAsync<PagedList<CourseSearchResponse>>(cacheKey);

			//if (cachedDataString != null)
			//{
			//	return new APIResponse
			//	{
			//		IsError = false,
			//		Payload = cachedDataString,
			//		Errors = null,
			//		Message = new MessageResponse
			//		{
			//			content = MessageCommon.GetSuccesfully,
			//			values = new Dictionary<string, string>() { { "name", "courses" } }
			//		}
			//	};
			//}

			var listCourse = await _courseRepository.GetListCourse();
			if(request.SearchRequest.KeywordName != null)
			{
				listCourse = listCourse.Where(x => ContentHelper.ConvertVietnameseToEnglish(x.Title.ToLower()).Contains(ContentHelper.ConvertVietnameseToEnglish(request.SearchRequest.KeywordName.ToLower()))).ToList();
			} else if(request.SearchRequest.DateTo != null)
			{
				listCourse = listCourse.Where(x => x.UpdatedAt <= request.SearchRequest.DateTo).ToList();
			}
			else if (request.SearchRequest.DateFrom != null)
			{
				listCourse = listCourse.Where(x => x.UpdatedAt >= request.SearchRequest.DateFrom).ToList();
			}
			else if (request.SearchRequest.DateFrom != null && request.SearchRequest.DateTo != null)
			{
				listCourse = listCourse.Where(x => x.UpdatedAt >= request.SearchRequest.DateFrom && x.UpdatedAt <= request.SearchRequest.DateTo).ToList();
			}
			else if (request.SearchRequest.Author != null)
			{
				listCourse = listCourse.Where(x => ContentHelper.ConvertVietnameseToEnglish(x.User.Username.ToLower()).Contains(ContentHelper.ConvertVietnameseToEnglish(request.SearchRequest.Author))).ToList();
			} else if (request.SearchRequest.Rating != null)
			{
				listCourse = listCourse.Where(x => x.CourseStatistic.Rating >= request.SearchRequest.Rating).ToList();
			} else if (request.SearchRequest.Sort != null)
			{
				var sortOptions = new Dictionary<SortCourse, Func<IQueryable<Course>, IOrderedQueryable<Course>>>
				{
					{ SortCourse.NewestCourses, listCourse => listCourse.OrderByDescending(x => x.CreatedAt) },
					{ SortCourse.MostReviews, listCourse => listCourse.OrderByDescending(x => x.CourseStatistic.TotalReview) },
					{ SortCourse.HighestRated, listCourse => listCourse.OrderByDescending(x => x.CourseStatistic.Rating) }
				};

					if (Enum.IsDefined(typeof(SortCourse), request.SearchRequest.Sort))
					{
						var sortType = (SortCourse)request.SearchRequest.Sort;

						// Check if the sortType exists in the dictionary
						if (sortOptions.TryGetValue(sortType, out var sortFunction))
						{
							// Apply the sorting function
							listCourse = sortFunction(listCourse.AsQueryable()).ToList();
						}
						else
						{
							// Handle default behavior for valid but unsupported sort types
							listCourse = listCourse.OrderByDescending(x => x.CreatedAt).ToList(); // Default sort
						}
					}
			}

			var listCourseResponse = _mapper.Map<List<CourseSearchResponse>>(listCourse); //Chưa check Discount Price
			foreach (var course in listCourseResponse)
			{
				var user = await _userRepository.GetById(course.CreatedBy); 
				course.Author = user!.Username!;

				var courseSta = await _courseStatisticRepository.GetByCourseId(course.Id);
				if(courseSta != null)
				{
					course.TotalLesson = courseSta.TotalLesson;
					course.TotalReview = courseSta.TotalReview;
					course.Rating = courseSta.Rating;
					course.TotalTime = courseSta.TotalTime;
				}
			}

			int totalItem = listCourseResponse.Count;
			var listPaged = listCourseResponse.Skip((request.PageNo - 1) * request.EachPage)
											.Take(request.EachPage)
											.ToList();
			var result = new PagedList<CourseSearchResponse>()
			{
				TotalItems = totalItem,
				Items = listPaged
			};

			//await _redisCaching.SetAsync(cacheKey, result, 60);
			//await _redisCaching.SetAsync(cacheSearchKey, request.SearchRequest.KeywordName, 4320);

			return new APIResponse
			{
				IsError = false,
				Payload = result,
				Errors = null,
				Message = new MessageResponse
				{
					content = MessageCommon.GetSuccesfully,
					values = new Dictionary<string, string>() { { "name", "courses"} }
				}
			};


		}
	}
}
