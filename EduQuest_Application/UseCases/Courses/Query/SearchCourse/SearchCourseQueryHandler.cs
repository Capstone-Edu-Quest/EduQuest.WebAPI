using AutoMapper;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.Courses.Queries.SearchCourse
{
    public class SearchCourseQueryHandler : IRequestHandler<SearchCourseQuery, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;
		private readonly ICourseStatisticRepository _courseStatisticRepository;
		private readonly ILearnerRepository _learnerRepository;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;
		private readonly IRedisCaching _redisCaching;

		public SearchCourseQueryHandler(ICourseRepository courseRepository, ICourseStatisticRepository courseStatisticRepository, ILearnerRepository learnerRepository, IUserRepository userRepository, IMapper mapper, IRedisCaching redisCaching)
		{
			_courseRepository = courseRepository;
			_courseStatisticRepository = courseStatisticRepository;
			_learnerRepository = learnerRepository;
			_userRepository = userRepository;
			_mapper = mapper;
			_redisCaching = redisCaching;
		}

		public async Task<APIResponse> Handle(SearchCourseQuery request, CancellationToken cancellationToken)
		{
            //string cacheKeyForRecommend = CourseHelper.GenerateCacheKeySearchCourse(request.SearchRequest, request.PageNo, request.EachPage);
            //string cacheKeyForSearch = CourseHelper.GenerateCacheKeySearchCourse(request.SearchRequest, request.PageNo, request.EachPage);
            //var cachedDataString = await _redisCaching.GetAsync<string>(cacheKeyForSearch);
            //if (cachedDataString != null)
            //{
            //	// Deserialize into a helper object first (using List<T> for Items)
            //	var jsonObject = JsonConvert.DeserializeObject<JObject>(cachedDataString);

            //	// Manually extract properties from the cached data
            //	var items = jsonObject["Items"].ToObject<List<CourseSearchResponse>>();
            //	var totalItems = jsonObject["TotalItems"].Value<int>();
            //	var currentPage = jsonObject["CurrentPage"].Value<int>();
            //	var eachPage = jsonObject["EachPage"].Value<int>();

            //	// Create the PagedList<EventResponseDto> manually
            //	var listData =  new PagedList<CourseSearchResponse>(
            //		items: items,
            //		totalItems: totalItems,
            //		currentPage: currentPage,
            //		eachPage: eachPage
            //	);
            //	return new APIResponse
            //	{
            //		IsError = false,
            //		Payload = listData,
            //		Errors = null,
            //		Message = new MessageResponse
            //		{
            //			content = MessageCommon.GetSuccesfully,
            //			values = new Dictionary<string, string>() { { "name", "courses" } }
            //		}
            //	};
            //}

            //var listCourse = await _courseRepository.GetListCourse();
            //if (request.SearchRequest.KeywordName != null)
            //{
            //	listCourse = listCourse.Where(x => ContentHelper.ConvertVietnameseToEnglish(x.Title.ToLower()).Contains(ContentHelper.ConvertVietnameseToEnglish(request.SearchRequest.KeywordName.ToLower()))).ToList();
            //} else if (request.SearchRequest.DateTo != null)
            //{
            //	listCourse = listCourse.Where(x => x.UpdatedAt <= request.SearchRequest.DateTo).ToList();
            //}
            //else if (request.SearchRequest.DateFrom != null)
            //{
            //	listCourse = listCourse.Where(x => x.UpdatedAt >= request.SearchRequest.DateFrom).ToList();
            //} else if (request.SearchRequest.IsPublic.HasValue)
            //{
            //	bool flag = request.SearchRequest.IsPublic.Value;

            //	listCourse = flag == true ? listCourse.Where(x => x.Status == StatusCourse.Public.ToString()).ToList()
            //		: listCourse.Where(x => x.Status != StatusCourse.Public.ToString()).ToList();
            //}
            //else if (request.SearchRequest.DateFrom != null && request.SearchRequest.DateTo != null)
            //{
            //	listCourse = listCourse.Where(x => x.UpdatedAt >= request.SearchRequest.DateFrom && x.UpdatedAt <= request.SearchRequest.DateTo).ToList();
            //}
            //else if (request.SearchRequest.Author != null)
            //{
            //	listCourse = listCourse.Where(x => ContentHelper.ConvertVietnameseToEnglish(x.User.Username.ToLower()).Contains(ContentHelper.ConvertVietnameseToEnglish(request.SearchRequest.Author))).ToList();
            //} else if (request.SearchRequest.Rating != null)
            //{
            //	listCourse = listCourse.Where(x => x.CourseStatistic.Rating >= request.SearchRequest.Rating).ToList();
            //}
            //	if (request.SearchRequest.TagListId != null && request.SearchRequest.TagListId.Any())
            //{
            //	listCourse = listCourse.Where(x => x.Tags.Any(tag => request.SearchRequest.TagListId.Contains(tag.Id))).ToList();
            //} else if (request.SearchRequest.Sort != null)
            //{
            //	var sortOptions = new Dictionary<SortCourse, Func<IQueryable<Course>, IOrderedQueryable<Course>>>
            //	{
            //		{ SortCourse.NewestCourses, listCourse => listCourse.OrderByDescending(x => x.CreatedAt) },
            //		{ SortCourse.MostReviews, listCourse => listCourse.OrderByDescending(x => x.CourseStatistic.TotalReview) },
            //		{ SortCourse.HighestRated, listCourse => listCourse.OrderByDescending(x => x.CourseStatistic.Rating) }
            //	};

            //		if (Enum.IsDefined(typeof(SortCourse), request.SearchRequest.Sort))
            //		{
            //			var sortType = (SortCourse)request.SearchRequest.Sort;

            //			// Check if the sortType exists in the dictionary
            //			if (sortOptions.TryGetValue(sortType, out var sortFunction))
            //			{
            //				// Apply the sorting function
            //				listCourse = sortFunction(listCourse.AsQueryable()).ToList();
            //			}
            //			else
            //			{
            //				// Handle default behavior for valid but unsupported sort types
            //				listCourse = listCourse.OrderByDescending(x => x.CreatedAt).ToList(); // Default sort
            //			}
            //		}
            //}

            //var listCourseResponse = _mapper.Map<List<CourseSearchResponse>>(listCourse); //Chưa check Discount Price
            //foreach (var course in listCourseResponse)
            //{
            //	var user = await _userRepository.GetById(course.CreatedBy); 
            //	course.Author = user!.Username!;

            //	var courseSta = await _courseStatisticRepository.GetByCourseId(course.Id);
            //	if(courseSta != null)
            //	{
            //		course.TotalLesson = (int)courseSta.TotalLesson;
            //		course.TotalReview = (int)courseSta.TotalReview;
            //		course.Rating = (int)courseSta.Rating;
            //		course.TotalTime = (int)courseSta.TotalTime;
            //	}

            //	var courseLeanrer = await _learnerRepository.GetByUserIdAndCourseId(request.UserId, course.Id);
            //	if (courseLeanrer != null)
            //	{
            //		course.ProgressPercentage = courseLeanrer.ProgressPercentage;
            //	}
            //	else
            //	{
            //		course.ProgressPercentage = null;
            //	}
            //}

            //int totalItem = listCourseResponse.Count;
            //var listPaged = listCourseResponse.Skip((request.PageNo - 1) * request.EachPage)
            //								.Take(request.EachPage)
            //								.ToList();
            //var result = new PagedList<CourseSearchResponse>()
            //{
            //	TotalItems = totalItem,
            //	Items = listPaged
            //};
            //var serializedPagedList = JsonConvert.SerializeObject(new
            //{
            //	Items = listPaged,
            //	TotalItems = totalItem,
            //	CurrentPage = request.PageNo,
            //	EachPage = request.EachPage
            //});

            //await _redisCaching.SetAsync(cacheKeyForSearch, serializedPagedList, 30);
            //await _redisCaching.SetAsync(cacheKeyForRecommend, serializedPagedList, 4320);


            var (courses, totalItems) = await _courseRepository.SearchCoursesAsync(request.SearchRequest, request.PageNo, request.EachPage);
          
            var courseResponses = _mapper.Map<List<CourseSearchResponse>>(courses);
            var courseIds = courseResponses.Select(c => c.Id).ToList();
            var learners = await _learnerRepository.GetByUserIdAndCourseIdsAsync(request.UserId, courseIds);
            var learnerDict = learners.ToDictionary(x => x.CourseId, x => x.ProgressPercentage);
            foreach (var course in courseResponses)
            {
                if (learnerDict.TryGetValue(course.Id, out var progress))
                    course.ProgressPercentage = progress;
                
            }
            if (request.SearchRequest.IsStudying is true)
            {
				courseResponses = courseResponses.Where(x => x.ProgressPercentage != null).ToList();
			} else if (request.SearchRequest.IsStudying is false)
            {
				courseResponses = courseResponses.Where(x => x.ProgressPercentage == null).ToList();
			}
            var result = new PagedList<CourseSearchResponse>
            {
                Items = courseResponses,
                TotalItems = courseResponses.Count(),
                CurrentPage = request.PageNo,
                EachPage = request.EachPage
            };

            return new APIResponse
            {
                IsError = false,
                Payload = result,
                Message = new MessageResponse
                {
                    content = MessageCommon.GetSuccesfully,
                    values = new Dictionary<string, string> { { "name", "courses" } }
                }
            };



        }
    }
}
