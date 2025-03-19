
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.FavoriteCourse.Command.AddFavoriteList
{
	public class UpdateFavoriteListCommandHandler : IRequestHandler<UpdateFavoriteListCommand, APIResponse>
	{
		private readonly IFavoriteListRepository _favoriteListRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICourseRepository _courseRepository;

		public UpdateFavoriteListCommandHandler(IFavoriteListRepository favoriteListRepository, IUnitOfWork unitOfWork, ICourseRepository courseRepository)
		{
			_favoriteListRepository = favoriteListRepository;
			_unitOfWork = unitOfWork;
			_courseRepository = courseRepository;
		}

		public async Task<APIResponse> Handle(UpdateFavoriteListCommand request, CancellationToken cancellationToken)
		{
			var fav = await _favoriteListRepository.GetFavoriteListByUserId(request.UserId);
			var listCourse = await _courseRepository.GetByListIds(request.CourseId);
			fav.Courses.Clear();
			fav.Courses = listCourse;
			await _favoriteListRepository.Update(fav);
			var result = await _unitOfWork.SaveChangesAsync() > 0;
			return new APIResponse
			{
				IsError = !result,
				Payload = result ? fav : null,
				Errors = result ? null : new ErrorResponse
				{
					StatusResponse = HttpStatusCode.BadRequest,
					StatusCode = (int)HttpStatusCode.BadRequest,
					Message = MessageCommon.UpdateFailed,
				},
				Message = new MessageResponse
				{
					content = result ? MessageCommon.UpdateSuccesfully : MessageCommon.UpdateFailed,
					values = new Dictionary<string, string> { { "name", "favorite course" } }
				}
			};
		}
	}
}
