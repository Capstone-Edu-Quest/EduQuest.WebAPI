using System.Net;
using AutoMapper;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.LearningPaths.Commands.EnrollLearningPath;

public class EnrollLearningPathHandler : IRequestHandler<EnrollLearningPathCommand, APIResponse>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEnrollerRepository _enrollerRepository;
    private const string Key = "name";
    private const string value = "learning path";

    public EnrollLearningPathHandler(ILearningPathRepository learningPathRepository, IMapper mapper, 
        IUserRepository userRepository, IUnitOfWork unitOfWork, IEnrollerRepository enrollerRepository)
    {
        _learningPathRepository = learningPathRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _enrollerRepository = enrollerRepository;
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
        #endregion
        DateTime now = DateTime.Now;
        int acummulateDate = 0;
        //validate Learing path exist 
        var learningPath = await _learningPathRepository.EnrollLearningPath(request.LearningPathId, request.UserId);
        if (learningPath == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed, MessageCommon.NotFound, Key, value);
        }
        var courses = await _learningPathRepository.GetLearningPathCourse(request.LearningPathId);
        if(learningPath.Enrollers != null && learningPath.Enrollers.FirstOrDefault(e => e.UserId == request.UserId) != null)
        {
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.AlreadyExists, null, Key, value);
        }
        // Calculate DueDate for each course based on the accumulated learning days
        List<Enroller> enrollers = new List<Enroller>();
        foreach (var lp in learningPath.LearningPathCourses.OrderBy(l => l.CourseOrder))
        {
            double? totalTime = 0;
            Enroller enroller = new Enroller();
            var course = courses.FirstOrDefault(c => c.Id == lp.CourseId);
            totalTime = course!.CourseStatistic.TotalTime;

            // Get the number of learning days required based on the total time
            int learningDate = GetLearningDate(totalTime);
            acummulateDate += learningDate;

            // Set the DueDate for the current course
            enroller.DueDate = now.AddDays(acummulateDate).ToUniversalTime();
            var learner = course.CourseLearners!.FirstOrDefault(c => c.UserId == request.UserId);
            if (learner != null && learner.ProgressPercentage >= 100)
            {
                enroller.IsCompleted = true;
            }
            enroller.CourseOrder = lp.CourseOrder;
            enroller.CourseId = lp.CourseId;
            enroller.LearningPathId = lp.LearningPathId;
            enroller.UserId = request.UserId;
            enroller.Id = Guid.NewGuid().ToString();
            enroller.CreatedAt = DateTime.Now.ToUniversalTime();
            enroller.EnrollDate = DateTime.Now.ToUniversalTime();
            enrollers.Add(enroller);
        }
        await _enrollerRepository.CreateRangeAsync(enrollers);
        await _unitOfWork.SaveChangesAsync();
        MyLearningPathResponse response = _mapper.Map<MyLearningPathResponse>(learningPath);
        response.CreatedBy = _mapper.Map<CommonUserResponse>(user);
        response.TotalCourses = learningPath.LearningPathCourses.Count;
        return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.UpdateSuccesfully,
            response, Key, value);
    }
    private int GetLearningDate(double? total)
    {
        int DailyLearningDay = MessageError.MinimumLearningTimeDaily;

        if (total == null || total < 1) 
        {
            return 1;
        }

        return (int)Math.Ceiling((double)total / DailyLearningDay);
    }
}
