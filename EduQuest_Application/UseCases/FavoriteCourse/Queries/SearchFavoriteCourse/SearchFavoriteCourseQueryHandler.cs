using AutoMapper;
using EduQuest_Application.DTO.Response;
using EduQuest_Application.Helper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;

namespace EduQuest_Application.UseCases.FavoriteCourse.Queries.SearchFavoriteCourse
{
	public class SearchFavoriteCourseQueryHandler : IRequestHandler<SearchFavoriteCourseQuery, APIResponse>
	{
		private readonly IFavoriteListRepository _favoriteListRepository;
		private readonly IMapper _mapper;

		public SearchFavoriteCourseQueryHandler(IFavoriteListRepository favoriteListRepository, IMapper mapper)
		{
			_favoriteListRepository = favoriteListRepository;
			_mapper = mapper;
		}

		public async Task<APIResponse> Handle(SearchFavoriteCourseQuery request, CancellationToken cancellationToken)
		{
			var listCourse = await _favoriteListRepository.GetFavoriteListByUserId(request.UserId);
			if(request.Name != null)
			{
				listCourse = listCourse.Where(x => SearchHelper.ConvertVietnameseToEnglish(x.Course.Title.ToLower()).Contains(SearchHelper.ConvertVietnameseToEnglish(request.Name.ToLower()))).ToList();
			}
			 var listResponse = new List<FavoriteCourseResponse>();
			listCourse.Select(course =>
			{
				var response = _mapper.Map<FavoriteCourseResponse>(course.Course);
				response.Author = course.User! != null ? new AuthorCourseResponse
				{
					Id = course.User.Id,
					Username = course.User.Username ?? string.Empty,
					Headline = course.User.Headline ?? string.Empty,
					Description = course.User.Description ?? string.Empty
				} : null;
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
