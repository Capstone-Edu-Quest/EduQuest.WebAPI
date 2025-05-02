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
    private const string Key = "name";
    private const string value = "learning path";

    public EnrollLearningPathHandler(ILearningPathRepository learningPathRepository, IMapper mapper, 
        IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _learningPathRepository = learningPathRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
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

        // Calculate DueDate for each course based on the accumulated learning days
        foreach (var lp in learningPath.LearningPathCourses.OrderBy(l => l.CourseOrder))
        {
            double? totalTime = 0;
            var course = courses.FirstOrDefault(c => c.Id == lp.CourseId);
            totalTime = course!.CourseStatistic.TotalTime;

            // Get the number of learning days required based on the total time
            int learningDate = GetLearningDate(totalTime);
            acummulateDate += learningDate;

            // Set the DueDate for the current course
            lp.DueDate = now.AddDays(acummulateDate);
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
