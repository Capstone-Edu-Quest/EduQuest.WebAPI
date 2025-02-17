using AutoMapper;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.DTO.Response;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Courses.Query.GetRecommendedCourse
{
	public class GetRecommendedCourseQueryHandler : IRequestHandler<GetRecommendedCourseQuery, APIResponse>
	{
		private readonly IRedisCaching _redisCaching;
		private readonly ICourseRepository _courseRepository;
		private readonly ICourseStatisticRepository _courseStatisticRepository;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;

		public GetRecommendedCourseQueryHandler(IRedisCaching redisCaching, ICourseRepository courseRepository, ICourseStatisticRepository courseStatisticRepository, IUserRepository userRepository, IMapper mapper)
		{
			_redisCaching = redisCaching;
			_courseRepository = courseRepository;
			_courseStatisticRepository = courseStatisticRepository;
			_userRepository = userRepository;
			_mapper = mapper;
		}

		public async Task<APIResponse> Handle(GetRecommendedCourseQuery request, CancellationToken cancellationToken)
		{
			var cacheSearchKey = $"searchHistory_{request.UserId}";
			var cachedDataString = await _redisCaching.SearchKeysAsync(cacheSearchKey);

			var listCourse = !string.IsNullOrEmpty(cacheSearchKey) ? await _courseRepository.GetCoursesByKeywordsAsync(cachedDataString) : (await _courseRepository.GetAll()).OrderByDescending(c => c.CreatedAt).ToList();

			var listCourseResponse = _mapper.Map<List<CourseSearchResponse>>(listCourse); //Chưa check Discount Price
			foreach (var course in listCourseResponse)
			{
				var user = await _userRepository.GetById(course.CreatedBy);
				course.Author = user!.Username!;

				var courseSta = await _courseStatisticRepository.GetByCourseId(course.Id);
				if (courseSta != null)
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

			return new APIResponse
			{
				IsError = false,
				Payload = result,
				Errors = null,
				Message = new MessageResponse
				{
					content = MessageCommon.GetSuccesfully,
					values = new Dictionary<string, string>() { { "name", "courses" } }
				}
			};
		}
	}
}
