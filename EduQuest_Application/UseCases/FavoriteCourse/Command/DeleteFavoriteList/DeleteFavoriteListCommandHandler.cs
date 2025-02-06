using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.FavoriteCourse.Command.DeleteFavoriteList
{
	public class DeleteFavoriteListCommandHandler : IRequestHandler<DeleteFavoriteListCommand, APIResponse>
	{
		private readonly IFavoriteListRepository _favoriteListRepository;

		public DeleteFavoriteListCommandHandler(IFavoriteListRepository favoriteListRepository)
		{
			_favoriteListRepository = favoriteListRepository;
		}

		public async Task<APIResponse> Handle(DeleteFavoriteListCommand request, CancellationToken cancellationToken)
		{
			var result = await _favoriteListRepository.DeleteFavList(request.UserId, request.CourseId);
			var response = new APIResponse
			{
				IsError = !result,
				Payload = result ? MessageCommon.DeleteSuccessfully : null,
				Errors = result ? null : new ErrorResponse
				{
					StatusResponse = HttpStatusCode.BadRequest,
					StatusCode = (int)HttpStatusCode.BadRequest,
					Message = MessageCommon.DeleteFailed,
				}
			};

			return response;
		}
	}
}
