using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.LearningPaths.Commands.EnrollLearningPath;

public class EnrollLearningPathHandler : IRequestHandler<EnrollLearningPathCommand, APIResponse>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private const string Key = "name";
    private const string value = "learning path";

    public EnrollLearningPathHandler(ILearningPathRepository learningPathRepository, IMapper mapper, 
        IUserRepository userRepository)
    {
        _learningPathRepository = learningPathRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(EnrollLearningPathCommand request, CancellationToken cancellationToken)
    {
        #region validate
        
        //validate owner
        User? user = await _userRepository.GetById(request.UserId);

        if (user == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed, MessageCommon.UserDontHavePer, Key, value);
        }

        bool isOwner = await _learningPathRepository.IsOwner(request.UserId, request.LearningPathId);
        bool isExpert = int.TryParse(user.RoleId, out int roleId) && roleId == (int)UserRole.Expert;

        if (!isOwner || isExpert)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed, MessageCommon.UserDontHavePer, Key, value);
        }
        #endregion

        //validate Learing path exist 
        var learningPath = await _learningPathRepository.EnrollLearningPath(request.LearningPathId);
        if (learningPath == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed, MessageCommon.NotFound, Key, value);
        }
        MyLearningPathResponse response = _mapper.Map<MyLearningPathResponse>(learningPath);
        response.CreatedBy = _mapper.Map<CommonUserResponse>(user);
        response.TotalCourses = learningPath.LearningPathCourses.Count;
        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.UpdateSuccesfully,
            response, Key, value);
    }
}
