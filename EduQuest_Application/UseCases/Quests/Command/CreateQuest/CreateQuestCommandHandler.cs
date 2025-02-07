using AutoMapper;
using EduQuest_Application.UseCases.Quests.Command.CreateQuest;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Achievements.Commands.CreateAchievement
{
	public class CreateQuestCommandHandler : IRequestHandler<CreateQuestCommand, APIResponse>
	{
		private readonly IQuestRepository _achievementRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IBadgeRepository _badgeRepository;

		public CreateQuestCommandHandler(IQuestRepository achievementRepository, IMapper mapper, IUnitOfWork unitOfWork, IBadgeRepository badgeRepository)
		{
			_achievementRepository = achievementRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_badgeRepository = badgeRepository;
		}

		public async Task<APIResponse> Handle(CreateQuestCommand request, CancellationToken cancellationToken)
		{
			var questEntity = _mapper.Map<Quest>(request.Quest);
			questEntity.Id = Guid.NewGuid().ToString();
			//foreach(var badId in request.Quest.ListBadgeId)
			//{
			//	var badge = await _badgeRepository.GetById(badId);
			//	questEntity.Badges.Add(badge!);
			//	await _unitOfWork.SaveChangesAsync();
			//}
			await _achievementRepository.Add(questEntity);
			var result = await _unitOfWork.SaveChangesAsync() > 0;
			return new APIResponse
			{
				IsError = !result,
				Payload = result ? questEntity : null,
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
