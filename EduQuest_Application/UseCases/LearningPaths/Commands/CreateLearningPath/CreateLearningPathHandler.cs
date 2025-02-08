using AutoMapper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.LearningPaths.Commands.CreateLearningPath;

public class CreateLearningPathHandler : IRequestHandler<CreateLearningPathCommand, APIResponse>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLearningPathHandler(ILearningPathRepository learningPathRepository, 
                                     IMapper mapper,
                                     ICourseRepository courseRepository,
                                     IUnitOfWork unitOfWork)
    {
        _learningPathRepository = learningPathRepository;
        _mapper = mapper;
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(CreateLearningPathCommand request, CancellationToken cancellationToken)
    {
        try
        {

            //parse data from json
            LearningPath learningPath = _mapper.Map<LearningPath>(request.CreateLearningPathRequest);
            List<LearningPathCourse> learningPathCourses = _mapper.Map<List<LearningPathCourse>>(request.CreateLearningPathRequest.Courses);

            #region filter duplicated courseId and duplicated course order
            int before = learningPathCourses.Count;
            var temp = learningPathCourses.AsQueryable();
            temp = temp.DistinctBy(t => t.CourseId);
            temp = temp.DistinctBy(t =>t.CourseOrder);
            learningPathCourses = temp.ToList();
            int after = learningPathCourses.Count;
            if(before > after)
            {
                return new APIResponse
                {
                    IsError = true,
                    Payload = null,
                    Errors = new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = MessageError.DuplicateCourseIdOrCourseOrder,
                        StatusResponse = HttpStatusCode.BadRequest
                    },
                    Message = new MessageResponse
                    {
                        content = MessageError.DuplicateCourseIdOrCourseOrder,
                        values = new Dictionary<string, string> { { "name", "learning path courses" } }
                    }
                };
            }
            #endregion
            int totalTime = 0;
            #region validate if any course is unavailable
            foreach (LearningPathCourse course in learningPathCourses)
            {
                if (!await _courseRepository.IsExist(course.CourseId))
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
                int courseTotalTime = await _courseRepository.GetTotalTime(course.CourseId);
                totalTime += courseTotalTime;
            }
            #endregion

            //parse values
            learningPath.EstimateDuration = totalTime.ToString();
            learningPath.IsDuplicated = false;
            learningPath.Id = Guid.NewGuid().ToString();
            learningPath.CreatedAt = DateTime.Now;
            learningPath.UserId = request.UserId;
            learningPath.LearningPathCourses = learningPathCourses;
            await _learningPathRepository.Add(learningPath);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return new APIResponse
                {
                    IsError = false,
                    Payload = learningPath,
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
                Payload = learningPath,
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
