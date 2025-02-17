using AutoMapper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.LearningMaterials.Command.CreateLeaningMaterial
{
    public class CreateLeaningMaterialCommandHandler : IRequestHandler<CreateLeaningMaterialCommand, APIResponse>
    {
        private readonly ILearningMaterialRepository _learningMaterialRepository;
        private readonly ISystemConfigRepository _systemConfigRepository;
        private readonly IStageRepository _stageRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateLeaningMaterialCommandHandler(ILearningMaterialRepository learningMaterialRepository, ISystemConfigRepository systemConfigRepository, IStageRepository stageRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _learningMaterialRepository = learningMaterialRepository;
            _systemConfigRepository = systemConfigRepository;
            _stageRepository = stageRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<APIResponse> Handle(CreateLeaningMaterialCommand request, CancellationToken cancellationToken)
        {
            if (request.Material == null || !request.Material.Any())
            {
                return new APIResponse
                {
                    IsError = true,
                    Errors = new ErrorResponse
                    {
                        StatusResponse = HttpStatusCode.NotFound,
                        StatusCode = (int)HttpStatusCode.NotFound,
                        Message = MessageCommon.CreateFailed,
                    },
                    Message = new MessageResponse
                    {
                        content = MessageCommon.NoProvided,
                        values = new Dictionary<string, string> { { "name", "learning material" } }
                    }
                };
            }
            var listMaterial = new List<LearningMaterial>();
			var stageIds = request.Material.SelectMany(m => m.StagesId).Distinct().ToList();
			var stageDictionary = (await _stageRepository.GetByIdsAsync(stageIds))
									.ToDictionary(s => s.Id, s => s);

			foreach (var item in request.Material)
            {
               foreach(var stageId in item.StagesId)
               {
					if (!stageDictionary.TryGetValue(stageId, out var stage))
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
								values = new Dictionary<string, string> { { "name", $"Stage ID {stageId}" } }
							}
						};
					}

					var material = _mapper.Map<LearningMaterial>(item);
					material.Id = Guid.NewGuid().ToString();
					material.Type = Enum.GetName(typeof(TypeOfLearningMetarial), item.Type);
                    material.StageId = stageId;

					var value = await _systemConfigRepository.GetByName(material.Type!);
					switch ((TypeOfLearningMetarial)item.Type!)
					{
						case TypeOfLearningMetarial.Docs:
							material.Duration = (int)value.Value!;
							break;
						case TypeOfLearningMetarial.Video:
							material.Duration = item.EstimateTime;
							break;
						case TypeOfLearningMetarial.Quiz:
							material.Duration = (int)(item.EstimateTime! * value.Value!);
							break;
						default:
							material.Duration = 0;
							break;
					}
					await _learningMaterialRepository.Add(material);
					listMaterial.Add(material);
				}
                

               


            }
            var result = await _unitOfWork.SaveChangesAsync() > 0;

            return new APIResponse
            {
                IsError = !result,
                Payload = result ? listMaterial : null,
                Errors = result ? null : new ErrorResponse
                {
                    StatusResponse = HttpStatusCode.BadRequest,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = MessageCommon.CreateFailed,
                },
                Message = new MessageResponse
                {
                    content = result ? MessageCommon.CreateSuccesfully : MessageCommon.CreateFailed,
                    values = new Dictionary<string, string> { { "name", "leaning material" } }
                }
            };
        }
    }
}
