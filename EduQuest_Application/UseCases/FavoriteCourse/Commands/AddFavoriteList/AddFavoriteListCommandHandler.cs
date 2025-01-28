using EduQuest_Application.UseCases.FavoriteCourse.Commands.DeleteFavoriteList;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.FavoriteCourse.Commands.AddFavoriteList
{
	public class AddFavoriteListCommandHandler : IRequestHandler<AddFavoriteListCommand, APIResponse>
	{
		private readonly IFavoriteListRepository _favoriteListRepository;
		private readonly IUnitOfWork _unitOfWork;

		public AddFavoriteListCommandHandler(IFavoriteListRepository favoriteListRepository, IUnitOfWork unitOfWork)
		{
			_favoriteListRepository = favoriteListRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<APIResponse> Handle(AddFavoriteListCommand request, CancellationToken cancellationToken)
		{
			var newFavCourse = new FavoriteList();
			newFavCourse.CourseId = request.CourseId;
			newFavCourse.UserId = request.UserId;
			await _favoriteListRepository.Add(newFavCourse);
			var result = await _unitOfWork.SaveChangesAsync() > 0;
			return new APIResponse
			{
				IsError = !result,
				Payload = result ? newFavCourse : null,
				Errors = result ? null : new ErrorResponse
				{
					StatusResponse = HttpStatusCode.BadRequest,
					StatusCode = (int)HttpStatusCode.BadRequest,
					Message = MessageCommon.SavingFailed,
				}
			};
		}
	}
}
