using AutoMapper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Domain.Entities;
using EduQuest_Application.Helper;

namespace EduQuest_Application.UseCases.LearningPaths.Commands.DeleteLearningPath;

public class DeleteLearningPathHandler : IRequestHandler<DeleteLearningPathCommand, APIResponse>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEnrollerRepository _enrollerRepository;
    private const string Key = "name";
    private const string value = "learning path";
    public DeleteLearningPathHandler(ILearningPathRepository learningPathRepository,
        IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository, IEnrollerRepository enrollerRepository)
    {
        _learningPathRepository = learningPathRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _enrollerRepository = enrollerRepository;
    }

    public async Task<APIResponse> Handle(DeleteLearningPathCommand request, CancellationToken cancellationToken)
    {
        try
        {
            #region validate ownership
            var user = await _userRepository.GetById(request.UserId);
            if (user == null)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.DeleteFailed, MessageCommon.UserDontHavePer, Key, value);
            }
            var learningPath = await _learningPathRepository.GetById(request.LearningPathId);
            
            if (learningPath == null)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.DeleteFailed, MessageCommon.NotFound, Key, value);
            }
            if (learningPath.UserId != user.Id)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.DeleteFailed, MessageCommon.UserDontHavePer, Key, value);
            }
            #endregion
            int coursesCount = learningPath.LearningPathCourses.Count();
            var enrollers = await _enrollerRepository.GetByLearningPathId(request.LearningPathId);
            if(enrollers != null)
            {
                _enrollerRepository.DeleteRange(enrollers);
            }
            await _learningPathRepository.Delete(learningPath.Id);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                CommonUserResponse userResponse = _mapper.Map<CommonUserResponse>(learningPath.User);
                MyLearningPathResponse myLearningPathResponse = _mapper.Map<MyLearningPathResponse>(learningPath);
                myLearningPathResponse.TotalCourses = coursesCount;
                myLearningPathResponse.CreatedBy = userResponse;
                return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK,MessageCommon.DeleteSuccessfully,
                    myLearningPathResponse, Key, value);
            }

            
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.DeleteFailed, MessageCommon.DeleteFailed, Key, value);
        
        }
        catch (Exception ex)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.DeleteFailed, ex.Message, Key, value);
        }
    }
}
