using AutoMapper;
using EduQuest_Application.DTO.Response;
using EduQuest_Application.Helper;
using EduQuest_Application.UseCases.FavoriteCourse.Query.SearchFavoriteCourse;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;

namespace EduQuest_Application.UseCases.FavoriteCourse.Queries.SearchFavoriteCourse
{
	public class SearchFavoriteCourseQueryHandler : IRequestHandler<SearchFavoriteCourseQuery, APIResponse>
	{
		private readonly IFavoriteListRepository _favoriteListRepository;
		private readonly IUserRepository _userRepository;
		private readonly ICourseStatisticRepository _courseStatisticRepository;
		private readonly IMapper _mapper;

		public SearchFavoriteCourseQueryHandler(IFavoriteListRepository favoriteListRepository, IUserRepository userRepository, ICourseStatisticRepository courseStatisticRepository, IMapper mapper)
		{
			_favoriteListRepository = favoriteListRepository;
			_userRepository = userRepository;
			_courseStatisticRepository = courseStatisticRepository;
			_mapper = mapper;
		}

		public async Task<APIResponse> Handle(SearchFavoriteCourseQuery request, CancellationToken cancellationToken)
		{
			var listCourse = await _favoriteListRepository.GetFavoriteListByUserId(request.UserId);
			if(request.Name != null)
			{
				listCourse = listCourse.Where(x => ContentHelper.ConvertVietnameseToEnglish(x.Course.Title.ToLower()).Contains(ContentHelper.ConvertVietnameseToEnglish(request.Name.ToLower()))).ToList();
			}
			var listResponse = new List<FavoriteCourseResponse>();
			listCourse.Select(async course =>
			{
				var response = _mapper.Map<FavoriteCourseResponse>(course.Course);
				var user = await _userRepository.GetById(course.UserId);
				var courseSta = await _courseStatisticRepository.GetByCourseId(course.Course.Id);

				//Mapping data
				response.Author = user!.Username!;
				response.Rating = courseSta.Rating;
				response.TotalLesson = courseSta.TotalLesson;
				response.TotalReview = courseSta.TotalReview;
				response.TotalTime = courseSta.TotalTime;	
				listResponse.Add(response);
				return response;
			}).ToList();

			return new APIResponse
			{
				IsError = false,
				Payload = listResponse,
				Errors = null,
			};
		}
	}
}
