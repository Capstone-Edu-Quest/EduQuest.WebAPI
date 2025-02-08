using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Domain.Repository;
using MediatR;
using EduQuest_Domain.Entities;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using EduQuest_Application.DTO.Request.LearningPaths;
using AutoMapper;
using EduQuest_Application.DTO.Response.LearningPaths;

namespace EduQuest_Application.UseCases.LearningPaths.Commands.DuplicateLearningPath;

public class DuplicateLearningPathHandler : IRequestHandler<DuplicateLearningPathCommand, APIResponse>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public DuplicateLearningPathHandler(ILearningPathRepository learningPathRepository, 
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IUserRepository userRepository)
    {
        _learningPathRepository = learningPathRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<APIResponse> Handle(DuplicateLearningPathCommand request, CancellationToken cancellationToken)
    {
        try
        {
            #region validate learning path is exist
            LearningPath? temp = await _learningPathRepository.GetLearningPathDetail(request.LearningPathId);
            if (temp == null)
            {
                return new APIResponse
                {
                    IsError = true,
                    Payload = null,
                    Errors = new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = MessageCommon.NotFound,
                        StatusResponse = HttpStatusCode.BadRequest
                    },
                    Message = new MessageResponse
                    {
                        content = MessageCommon.CreateFailed,
                        values = new Dictionary<string, string> { { "name", "learning path" } }
                    }
                };
            }
            #endregion

            //parse values
            LearningPath newLearningPath = new LearningPath();
            newLearningPath.Id = Guid.NewGuid().ToString();
            newLearningPath.UserId = request.UserId;
            newLearningPath.IsEnrolled = false;
            newLearningPath.IsPublic = temp.IsPublic;
            newLearningPath.CreatedAt = DateTime.Now;
            newLearningPath.Name = temp.Name;
            newLearningPath.Description = temp.Description;
            newLearningPath.TotalTimes = temp.TotalTimes;
            List<CreateCourseLearningPath> newLPC = _mapper.Map<List<CreateCourseLearningPath>>(temp.LearningPathCourses);
            List<LearningPathCourse> learningPathCourses = _mapper.Map<List<LearningPathCourse>>(newLPC);
            newLearningPath.LearningPathCourses = learningPathCourses;

            //saving
            await _learningPathRepository.Add(newLearningPath);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                User user = await _userRepository.GetById(request.UserId)!;
                CommonUserResponse userResponse = _mapper.Map<CommonUserResponse>(user);
                MyLearningPathResponse myLearningPathResponse = _mapper.Map<MyLearningPathResponse>(newLearningPath);
                myLearningPathResponse.TotalCourses = newLearningPath.LearningPathCourses.Count;
                myLearningPathResponse.CreatedBy = userResponse;
                return new APIResponse
                {
                    IsError = false,
                    Payload = myLearningPathResponse,
                    Errors = null,
                    Message = new MessageResponse
                    {
                        content = MessageCommon.CreateSuccesfully,
                        values = new Dictionary<string, string> { { "name", "learning path" } }
                    }
                };
            }

            return new APIResponse
            {
                IsError = false,
                Payload = temp,
                Errors = null,
                Message = new MessageResponse
                {
                    content = MessageCommon.CreateFailed,
                    values = new Dictionary<string, string> { { "name", "learning path" } }
                }
            };
        }
        catch (Exception ex)
        {
            return new APIResponse
            {
                IsError = true,
                Payload = null,
                Errors = new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = ex.Message,
                    StatusResponse = HttpStatusCode.BadRequest
                },
                Message = new MessageResponse
                {
                    content = MessageCommon.CreateFailed,
                    values = new Dictionary<string, string> { { "name", "learning path" } }
                }
            };
        }
    }
}
