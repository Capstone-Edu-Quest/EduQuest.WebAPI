using AutoMapper;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.LearningPaths.Commands.CreateLearningPath;

public class CreateLearningPathHandler : IRequestHandler<CreateLearningPathCommand, APIResponse>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private const string Key = "name";
    private const string value = "learning path";
    public CreateLearningPathHandler(ILearningPathRepository learningPathRepository, 
                                     IMapper mapper,
                                     IUserRepository userRepository,
                                     ICourseRepository courseRepository,
                                     IUnitOfWork unitOfWork)
    {
        _learningPathRepository = learningPathRepository;
        _mapper = mapper;
        _courseRepository = courseRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<APIResponse> Handle(CreateLearningPathCommand request, CancellationToken cancellationToken)
    {
        try
        {

            //parse data from json
            LearningPath learningPath = _mapper.Map<LearningPath>(request.CreateLearningPathRequest);
            List<LearningPathCourse> learningPathCourses = _mapper.Map<List<LearningPathCourse>>(request.CreateLearningPathRequest.Courses);

            //validate if user is exsist
            User? user = await _userRepository.GetById(request.UserId)!;
            if(user == null)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.CreateFailed, MessageCommon.Unauthorized, Key, value);
            }
            

            #region filter duplicated courseId and duplicated course order
            int before = learningPathCourses.Count;
            var temp = learningPathCourses.AsQueryable();
            temp = temp.DistinctBy(t => t.CourseId);
            temp = temp.DistinctBy(t =>t.CourseOrder);
            learningPathCourses = temp.ToList();
            int after = learningPathCourses.Count;
            if(before > after)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.CreateFailed, MessageError.DuplicateCourseIdOrCourseOrder, Key, "learning path courses");
            }
            #endregion
            int totalTime = 0;
            List<string> courseIds = new List<string>();
            #region validate if any course is unavailable
            foreach (LearningPathCourse course in learningPathCourses)
            {
                if (!await _courseRepository.IsExist(course.CourseId))
                {
                    return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.CreateFailed, MessageCommon.NotFound, Key, value);
                }
                courseIds.Add(course.CourseId);
                int courseTotalTime = await _courseRepository.GetTotalTime(course.CourseId);
                totalTime += courseTotalTime;
            }
            #endregion

            //parse values
            learningPath.CreatedByExpert = true ? int.Parse(user.RoleId!) == (int)UserRole.Expert : false;            
            learningPath.TotalTimes = totalTime;
            learningPath.IsEnrolled = false;
            learningPath.Id = Guid.NewGuid().ToString();
            learningPath.CreatedAt = DateTime.Now.ToUniversalTime();
            learningPath.UserId = request.UserId;
            learningPath.LearningPathCourses = learningPathCourses;
            //getTag list to update LearningPath tag
            List<Tag> tags = await _courseRepository.GetTagByCourseIds(courseIds);
            learningPath.Tags = tags;
            await _learningPathRepository.Add(learningPath);
            
            
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                CommonUserResponse userResponse = _mapper.Map<CommonUserResponse>(user);
                MyLearningPathResponse myLearningPathResponse = _mapper.Map<MyLearningPathResponse>(learningPath);
                myLearningPathResponse.TotalCourses = learningPath.LearningPathCourses.Count;
                myLearningPathResponse.CreatedBy = userResponse;

                return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.CreateSuccesfully,
                    myLearningPathResponse, Key, value);
            }

            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.DeleteFailed, MessageCommon.CreateFailed, Key, value);
        
        }
        catch (Exception ex)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.CreateFailed, ex.Message, Key, value);
        }
    }
}
