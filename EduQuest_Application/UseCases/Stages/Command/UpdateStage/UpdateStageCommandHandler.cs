using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Stages.Command.UpdateStage
{
	public class UpdateStageCommandHandler : IRequestHandler<UpdateStageCommand, APIResponse>
	{
		private readonly ILessonRepository _stageRepository;
		private readonly IUnitOfWork _unitOfWork;

		public UpdateStageCommandHandler(ILessonRepository stageRepository, IUnitOfWork unitOfWork)
		{
			_stageRepository = stageRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<APIResponse> Handle(UpdateStageCommand request, CancellationToken cancellationToken)
		{
			if (request.Stages == null || !request.Stages.Any())
			{
				return new APIResponse
				{
					IsError = true,
					Errors = new ErrorResponse
					{
						StatusResponse = HttpStatusCode.NotFound,
						StatusCode = (int)HttpStatusCode.NotFound,
						Message = MessageCommon.UpdateFailed,
					},
					Message = new MessageResponse
					{
						content = MessageCommon.NoProvided,
						values = new Dictionary<string, string> { { "name", "stage" } }
					}
				};
			}
			var listStage = new List<Lesson>();
			foreach (var stageRequest in request.Stages)
			{
				var existingStage = await _stageRepository.GetById(stageRequest.Id);
				if (existingStage == null)
				{
					return new APIResponse
					{
						IsError = true,
						Errors = new ErrorResponse
						{
							StatusResponse = HttpStatusCode.NotFound,
							StatusCode = (int)HttpStatusCode.NotFound,
							Message = MessageCommon.NotFound,
						},
						Message = new MessageResponse
						{
							content = MessageCommon.NotFound,
							values = new Dictionary<string, string> { { "name", $"stageID {stageRequest.Id}" } }
						}
					};
				}
				


				existingStage.Name = stageRequest.Name ?? existingStage.Name;
				existingStage.Description = stageRequest.Description ?? existingStage.Description;
				listStage.Add(existingStage);
				
			}
			await _stageRepository.UpdateRangeAsync(listStage);
			var result = await _unitOfWork.SaveChangesAsync() > 0;

			return new APIResponse
			{
				IsError = !result,
				Payload = result ? request.Stages : null,
				Errors = result ? null : new ErrorResponse
				{
					StatusResponse = HttpStatusCode.BadRequest,
					StatusCode = (int)HttpStatusCode.BadRequest,
					Message = MessageCommon.UpdateFailed,
				},
				Message = new MessageResponse
				{
					content = result ? MessageCommon.UpdateSuccesfully : MessageCommon.UpdateFailed,
					values = new Dictionary<string, string> { { "name", "stages" } }
				}
			};
		}
	}
}
