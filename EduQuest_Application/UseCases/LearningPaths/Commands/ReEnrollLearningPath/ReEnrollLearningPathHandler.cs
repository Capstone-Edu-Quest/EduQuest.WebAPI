using AutoMapper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Domain.Repository;
using MediatR;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using static EduQuest_Domain.Constants.Constants;
using System.Net;

namespace EduQuest_Application.UseCases.LearningPaths.Commands.ReEnrollLearningPath;

public class ReEnrollLearningPathHandler : IRequestHandler<ReEnrollLearningPathCommand, APIResponse>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEnrollerRepository _enrollerRepository;
    private const string Key = "name";
    private const string value = "learning path";

    public ReEnrollLearningPathHandler(ILearningPathRepository learningPathRepository, IUserRepository userRepository, 
        IUnitOfWork unitOfWork, IMapper mapper, IEnrollerRepository enrollerRepository)
    {
        _learningPathRepository = learningPathRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _enrollerRepository = enrollerRepository;
    }

    public async Task<APIResponse> Handle(ReEnrollLearningPathCommand request, CancellationToken cancellationToken)
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
        var learningPath = await _learningPathRepository.GetById(request.LearningPathId);
        if (learningPath == null)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed, MessageCommon.NotFound, Key, value);
        }
        var courses = await _learningPathRepository.GetLearningPathCourse(request.LearningPathId);
        if (learningPath.Enrollers == null || learningPath.Enrollers.FirstOrDefault(e => e.UserId == request.UserId) == null)
        {
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.NotFound, null, Key, value);
        }
        // Calculate DueDate for each course based on the accumulated learning days
        var enrollers = await _enrollerRepository.GetByLearningPathId(request.LearningPathId, request.UserId);
        foreach (var lp in learningPath.LearningPathCourses.OrderBy(l => l.CourseOrder))
        {
            double? totalTime = 0;
            Enroller enroller = enrollers!
                .Where(e => e.CourseId! == lp.CourseId!)
                .FirstOrDefault()!;
            if(enroller.IsOverDue)
            {
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
                enroller.IsOverDue = false;
            }
        }
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

        // Calculate the number of full learning days
        int temp = Convert.ToInt32(total / DailyLearningDay);

        // Add an extra day if there is remaining time to learn
        int sub = (total % DailyLearningDay) > 0 ? 1 : 0;
        return temp + sub;
    }
}

