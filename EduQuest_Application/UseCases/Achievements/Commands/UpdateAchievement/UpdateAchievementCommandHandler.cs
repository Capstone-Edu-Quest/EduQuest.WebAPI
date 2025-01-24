using AutoMapper;
using EduQuest_Application.UseCases.Achievements.Commands.CreateAchievement;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace EduQuest_Application.UseCases.Achievements.Commands.UpdateAchievement
{
	public class UpdateAchievementCommandHandler : IRequestHandler<UpdateAchievementCommand, APIResponse>
	{
		private readonly IAchievementRepository _achievementRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IBadgeRepository _badgeRepository;

		

		public async Task<APIResponse> Handle(UpdateAchievementCommand request, CancellationToken cancellationToken)
		{
			var achiExisted = await _achievementRepository.GetAchievementById(request.Achievement.Id);
			if(achiExisted.Users == null)
			{
				return new APIResponse
				{
					IsError = true,
					Payload = achiExisted,
					Errors = new ErrorResponse
					{
						StatusResponse = HttpStatusCode.BadRequest,
						StatusCode = (int)HttpStatusCode.BadRequest,
						Message = MessageAchievement.ExistedUser,
					}
				};
			}
			achiExisted.Name = request.Achievement.Name;
			achiExisted.Description = request.Achievement.Description;
			achiExisted.Badges.Clear();
			foreach (var badId in request.Achievement.ListBadgeId)
			{
				var badge = await _badgeRepository.GetById(badId);
				achiExisted.Badges.Add(badge!);
				await _unitOfWork.SaveChangesAsync();
			}
			await _achievementRepository.Add(achiExisted);
			var result = await _unitOfWork.SaveChangesAsync() > 0;
			return new APIResponse
			{
				IsError = !result,
				Payload = result ? achiExisted : null,
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
