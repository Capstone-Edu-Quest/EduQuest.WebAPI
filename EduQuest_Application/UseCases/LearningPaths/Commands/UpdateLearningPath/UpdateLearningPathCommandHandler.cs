

using AutoMapper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.UseCases.LearningPaths.Commands.UpdateLearningPath;

public class UpdateLearningPathCommandHandler : IRequestHandler<UpdateLearningPathCommand, APIResponse>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLearningPathCommandHandler(ILearningPathRepository learningPathRepository, 
        ICourseRepository courseRepository, IUserRepository userRepository,
        IMapper mapper, IUnitOfWork unitOfWork)
    {
        _learningPathRepository = learningPathRepository;
        _courseRepository = courseRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task<APIResponse> Handle(UpdateLearningPathCommand request, CancellationToken cancellationToken)
    {
        #region validate
        //validate Learing path exist 
        var learingPath = await _learningPathRepository.GetLearningPathDetail(request.LearningPathId);
        if (learingPath == null)
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
                    content = MessageCommon.UpdateFailed,
                    values = new Dictionary<string, string> { { "name", "learning path" } }
                }
            };
        }
        //validate owner
        if(!await _learningPathRepository.IsOwner(request.UserId, request.LearningPathId))
        {
            return new APIResponse
            {
                IsError = true,
                Payload = null,
                Errors = new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = MessageCommon.UserDontHavePer,
                    StatusResponse = HttpStatusCode.BadRequest
                },
                Message = new MessageResponse
                {
                    content = MessageCommon.UpdateFailed,
                    values = new Dictionary<string, string> { { "name", "learning path" } }
                }
            };
        }
        #endregion

        learingPath.Name = request.LearningPathRequest.Name;
        learingPath.Description = request.LearningPathRequest.Description;
        learingPath.IsPublic = request.LearningPathRequest.IsPublic;
        learingPath.UpdatedAt = DateTime.Now;
        learingPath.UpdatedBy = request.UserId;

        //handler learning path courses
        var courses = learingPath.LearningPathCourses;
        foreach(var updatecourse in request.LearningPathRequest.Courses)
        {
            
            if(updatecourse.Action == "add")
            {
                updatecourse.CourseOrder = courses.Count;
                LearningPathCourse temp = courses.FirstOrDefault(c => c.CourseOrder == updatecourse.CourseOrder)!;
                if (temp != null) updatecourse.CourseOrder += 1;
                learingPath.LearningPathCourses.Add(_mapper.Map<LearningPathCourse>(updatecourse));
            }
            if(updatecourse.Action == "delete")
            {
                LearningPathCourse temp = courses.FirstOrDefault(c => c.CourseId == updatecourse.CourseId)!;
                learingPath.LearningPathCourses.Remove(temp);
            }
            if(updatecourse.Action == "update")
            {
                LearningPathCourse temp = courses.FirstOrDefault(c => c.CourseId == updatecourse.CourseId)!;
                temp.CourseOrder = updatecourse.CourseOrder;
            }
        }
        //rearrange course order
        var orderedCourses = learingPath.LearningPathCourses
        .OrderBy(c => c.CourseOrder)
        .ToList();
        for (int i = 0; i < orderedCourses.Count; i++)
        {
            orderedCourses[i].CourseOrder = i + 1;
        }
        await _learningPathRepository.Update(learingPath);
        if(await _unitOfWork.SaveChangesAsync() > 0)
        {
            return new APIResponse
            {
                IsError = false,
                Payload = learingPath,
                Errors = null,
                Message = new MessageResponse
                {
                    content = MessageCommon.UpdateSuccesfully,
                    values = new Dictionary<string, string> { { "name", "learning path" } }
                }
            };
        }
        return new APIResponse
        {
            IsError = true,
            Payload = null,
            Errors = new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = MessageCommon.UpdateFailed,
                StatusResponse = HttpStatusCode.BadRequest
            },
            Message = new MessageResponse
            {
                content = MessageCommon.UpdateFailed,
                values = new Dictionary<string, string> { { "name", "learning path" } }
            }
        };
    }
}
