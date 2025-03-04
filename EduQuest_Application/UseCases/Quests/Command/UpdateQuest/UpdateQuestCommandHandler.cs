using AutoMapper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Achievements.Commands.UpdateAchievement;

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
		throw new NotImplementedException();
	}
}
