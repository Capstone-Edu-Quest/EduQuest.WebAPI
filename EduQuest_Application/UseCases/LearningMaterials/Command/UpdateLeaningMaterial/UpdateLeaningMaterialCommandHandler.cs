using AutoMapper;
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
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.LearningMaterials.Command.UpdateLeaningMaterial
{
	public class UpdateLeaningMaterialCommandHandler : IRequestHandler<UpdateLeaningMaterialCommand, APIResponse>
	{
		private readonly IMaterialRepository _learningMaterialRepository;
		private readonly ISystemConfigRepository _systemConfigRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public UpdateLeaningMaterialCommandHandler(IMaterialRepository learningMaterialRepository, ISystemConfigRepository systemConfigRepository, IUnitOfWork unitOfWork, IMapper mapper)
		{
			_learningMaterialRepository = learningMaterialRepository;
			_systemConfigRepository = systemConfigRepository;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<APIResponse> Handle(UpdateLeaningMaterialCommand request, CancellationToken cancellationToken)
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
						Message = MessageCommon.UpdateFailed,
					},
					Message = new MessageResponse
					{
						content = MessageCommon.NoProvided,
						values = new Dictionary<string, string> { { "name", "learning material" } }
					}
				};
			}

			var listUpdated = new List<Material>();
			foreach(var item in request.Material)
			{
				var materialExisted = await _learningMaterialRepository.GetById(item.Id);
				if (materialExisted != null)
				{
					var materialUpdate = _mapper.Map<Material>(item);
					var typeToUpdate = Enum.GetName(typeof(TypeOfMaterial), item.Type!);

					if (materialExisted.Type != typeToUpdate)
					{
						materialExisted.Type = typeToUpdate!;
						var value = await _systemConfigRepository.GetByName(typeToUpdate!);
						switch ((TypeOfMaterial)item.Type!)
						{
							case TypeOfMaterial.Document:
								materialUpdate.Duration = (int)value.Value!;
								break;
							case TypeOfMaterial.Video:
								materialUpdate.Duration = item.EstimateTime;
								break;
							case TypeOfMaterial.Quiz:
								materialUpdate.Duration = (int)(item.EstimateTime! * value.Value!);
								break;
							default:
								materialUpdate.Duration = 0;
								break;
						}
					}

					await _learningMaterialRepository.Update(materialUpdate);
					listUpdated.Add(materialUpdate);
				} else
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
							values = new Dictionary<string, string> { { "name", $"Learning meterial ID {item.Id}" } }
						}
					};
				}
			}
			var result = await _unitOfWork.SaveChangesAsync() > 0;

			return new APIResponse
			{
				IsError = !result,
				Payload = result ? listUpdated : null,
				Errors = result ? null : new ErrorResponse
				{
					StatusResponse = HttpStatusCode.BadRequest,
					StatusCode = (int)HttpStatusCode.BadRequest,
					Message = MessageCommon.UpdateFailed,
				},
				Message = new MessageResponse
				{
					content = result ? MessageCommon.UpdateSuccesfully : MessageCommon.UpdateFailed,
					values = new Dictionary<string, string> { { "name", "learning material" } }
				}
			};
		}
	}
}
