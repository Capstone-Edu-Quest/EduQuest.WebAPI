

using AutoMapper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Domain.Entities;
using EduQuest_Application.Helper;

namespace EduQuest_Application.UseCases.LearningPaths.Queries.GetLearningPathDetail;

public class GetLearningPathDetailHandler : IRequestHandler<GetLearningPathDetailQuery, APIResponse>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly IMapper _mapper;
    private const string Key = "name";
    private const string value = "learning path";
    public GetLearningPathDetailHandler(ILearningPathRepository learningPathRepository, IMapper mapper)
    {
        _learningPathRepository = learningPathRepository;
        _mapper = mapper;
    }

    public async Task<APIResponse> Handle(GetLearningPathDetailQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var learningPath = await _learningPathRepository.GetLearningPathDetail(request.LearningPathId);

            if (learningPath == null)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.GetFailed, MessageCommon.NotFound, Key, value);
            }

            LearningPathDetailResponse response = _mapper.Map<LearningPathDetailResponse>(learningPath);

            //get course list
            List<Course> courses = await _learningPathRepository.GetLearningPathCourse(request.LearningPathId);

            // Create list of LearningPathCourseResponse whit Order field
            var learningPathCourses = courses.Select(course =>
            {
                var learningPathCourse = _mapper.Map<LearningPathCourseResponse>(course);
                var tempCourse = learningPath.LearningPathCourses.FirstOrDefault(r => r.CourseId == course.Id);

                // parse order from LearningPathCourse Entity
                learningPathCourse.Order = tempCourse?.CourseOrder ?? -1; // -1 if not found
                learningPathCourse.RequirementList = course.Requirement.Split(",").ToList();
                learningPathCourse.Author = course.User.Username;
                if(!string.IsNullOrEmpty(request.UserId))
                {
                    var learner = course.CourseLearners.Where(c => c.UserId == request.UserId).FirstOrDefault();
                    if (learner != null)
                    {
                        learningPathCourse.ProgressPercentage = learner.ProgressPercentage.Value;
                    }
                    else
                    {
                        learningPathCourse.ProgressPercentage = -1;
                    }
                    var lp = learningPath.Enrollers
                    .FirstOrDefault(c => c.CourseId == course.Id && c.UserId == request.UserId && c.LearningPathId == learningPath.Id);
                    if(lp != null)
                    {
                        learningPathCourse.DueDate = lp.DueDate != null ? lp.DueDate.Value : null;
                        learningPathCourse.IsOverDue = lp.IsOverDue;
                        learningPathCourse.IsCompleted = lp.IsCompleted;
                    }
                    else
                    {
                        learningPathCourse.DueDate = null;
                        learningPathCourse.IsOverDue = false;
                        learningPathCourse.IsCompleted = false;
                    }
                    
                }
                /*else
                {
                    learningPathCourse.ProgressPercentage = -1;
                }*/
                return learningPathCourse;
            }).ToList();

            // parse learningPathCourses, CreatedBy and TotalCourses
            if (!string.IsNullOrEmpty(request.UserId))
            {
                var enroll = learningPath.Enrollers.Where(l => l.UserId == request.UserId).FirstOrDefault();
                response.IsEnrolled = enroll != null;
            }
            else
            {
                response.IsEnrolled = false;
            }
            
            response.TotalCourses = learningPathCourses.Count;
            response.Courses = learningPathCourses.OrderBy(r => r.Order).ToList();
            response.CreatedBy = _mapper.Map<CommonUserResponse>(learningPath.User);
            response.TotalEnroller = learningPath.Enrollers.DistinctBy(l => l.UserId).Count();
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.GetSuccesfully,
                response, Key, value);
        }
        catch (Exception ex)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.GetFailed, ex.Message, Key, value);
        }
    }
}
