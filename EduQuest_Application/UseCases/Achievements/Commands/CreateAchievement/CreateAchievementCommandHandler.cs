using AutoMapper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Achievements.Commands.CreateAchievement
{
	public class CreateAchievementCommandHandler : IRequestHandler<CreateAchievementCommand, APIResponse>
	{
		private readonly IAchievementRepository _achievementRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IBadgeRepository _badgeRepository;

		public CreateAchievementCommandHandler(IAchievementRepository achievementRepository, IMapper mapper, IUnitOfWork unitOfWork, IBadgeRepository badgeRepository)
		{
			_achievementRepository = achievementRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_badgeRepository = badgeRepository;
		}

		public async Task<APIResponse> Handle(CreateAchievementCommand request, CancellationToken cancellationToken)
		{
			var achiEntity = _mapper.Map<Achievement>(request.Achievement);
			achiEntity.Id = Guid.NewGuid().ToString();
			foreach(var badId in request.Achievement.ListBadgeId)
			{
				var badge = await _badgeRepository.GetById(badId);
				achiEntity.Badges.Add(badge!);
				await _unitOfWork.SaveChangesAsync();
			}
			await _achievementRepository.Add(achiEntity);
			var result = await _unitOfWork.SaveChangesAsync() > 0;
			return new APIResponse
			{
				IsError = !result,
				Payload = result ? achiEntity : null,
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
