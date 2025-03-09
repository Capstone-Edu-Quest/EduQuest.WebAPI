using AutoMapper;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.DTO.Response.Quests;
using EduQuest_Application.ExternalServices.QuartzService;
using EduQuest_Application.Helper;
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
		private readonly IUserQuestRepository _userQuestRepository;
		private readonly IUserRepository _userRepository;
		private readonly IQuartzService _quartzService;
		private const string key = "name";
		private const string value = "quest";

        public CreateQuestCommandHandler(IQuestRepository achievementRepository, IMapper mapper, IUnitOfWork unitOfWork,
			IUserQuestRepository userQuestRepository, IUserRepository userRepository,
            IQuartzService qquartzService)
		{
			_achievementRepository = achievementRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_userQuestRepository = userQuestRepository;
			_userRepository = userRepository;
			_quartzService = qquartzService;
		}

		public async Task<APIResponse> Handle(CreateQuestCommand request, CancellationToken cancellationToken)
		{
			User? user = await _userRepository.GetById(request.UserId);
			if (user == null)
			{
				return GeneralHelper.CreateErrorResponse(HttpStatusCode.Unauthorized, MessageCommon.CreateFailed, MessageCommon.Unauthorized, key, value);
			}

			var questEntity = _mapper.Map<Quest>(request.Quest);
			questEntity.Id = Guid.NewGuid().ToString();
			questEntity.CreatedAt = DateTime.Now.ToUniversalTime();
			questEntity.CreatedBy = request.UserId;
            await _achievementRepository.Add(questEntity);
			var result = await _unitOfWork.SaveChangesAsync() > 0;
			if(result)
			{
                //add quest to all user.
				await _quartzService.AddNewQuestToAllUser(questEntity.Id);
				//await _userQuestRepository.AddNewQuestToAllUseQuest(questEntity);

				//Create response
				QuestResponse response = _mapper.Map<QuestResponse>(questEntity);
				response.CreatedByUser = _mapper.Map<CommonUserResponse>(user);
				return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.CreateSuccesfully, response, key, value);
            }
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.CreateFailed, MessageCommon.CreateFailed, key, value);
        }
	}
}
