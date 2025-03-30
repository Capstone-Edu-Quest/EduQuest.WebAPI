using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EduQuest_Domain.Entities;
using EduQuest_Application.Helper;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using AutoMapper;
using EduQuest_Application.DTO.Response.Levels;

namespace EduQuest_Application.UseCases.Level.Command.DeleteLevel;

public class DeleteLevelHandler : IRequestHandler<DeleteLevelCommand, APIResponse>
{
    private readonly ILevelRepository _levelRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteLevelHandler(ILevelRepository levelRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _levelRepository = levelRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(DeleteLevelCommand request, CancellationToken cancellationToken)
    {
        Levels? existLevel = await _levelRepository.GetById(request.Id);
        if(existLevel == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.NotFound,
                MessageCommon.NotFound, "name", "level");
        }
        int level = existLevel.Level;
        await _levelRepository.Delete(request.Id);
        if(await _unitOfWork.SaveChangesAsync() > 0)
        {
            await _levelRepository.ReArrangeLevelAfterDelete(level);
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.DeleteSuccessfully,
            _mapper.Map<LevelResponseDto>(existLevel), "name", "level");
        }
        return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.NotFound,
                MessageCommon.NotFound, "name", "level");
    }
}
