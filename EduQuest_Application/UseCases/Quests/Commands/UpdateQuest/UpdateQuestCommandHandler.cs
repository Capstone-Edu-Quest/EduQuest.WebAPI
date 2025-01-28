using AutoMapper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Achievements.Commands.UpdateAchievement
{
	public class UpdateQuestCommandHandler : IRequestHandler<UpdateQuestCommand, APIResponse>
	{
		private readonly IQuestRepository _questRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IBadgeRepository _badgeRepository;

		public UpdateQuestCommandHandler(IQuestRepository questRepository, IMapper mapper, IUnitOfWork unitOfWork, IBadgeRepository badgeRepository)
		{
			_questRepository = questRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_badgeRepository = badgeRepository;
		}

		public async Task<APIResponse> Handle(UpdateQuestCommand request, CancellationToken cancellationToken)
		{
			var achiExisted = await _questRepository.GetQuestById(request.Quest.Id);
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
			achiExisted.Name = request.Quest.Name;
			achiExisted.Description = request.Quest.Description;
			//achiExisted.Badges.Clear();
			//foreach (var badId in request.Quest.ListBadgeId)
			//{
			//	var badge = await _badgeRepository.GetById(badId);
			//	achiExisted.Badges.Add(badge!);
			//	await _unitOfWork.SaveChangesAsync();
			//}
			await _questRepository.Add(achiExisted);
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
