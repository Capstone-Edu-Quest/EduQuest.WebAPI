

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
                return learningPathCourse;
            }).ToList();

            // parse learningPathCourses, CreatedBy and TotalCourses
            response.TotalCourses = learningPathCourses.Count;
            response.Courses = learningPathCourses.OrderBy(r => r.Order).ToList();
            response.Author = learningPath.User.Username!;
            response.CreatedBy = learningPath.UserId;
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK,MessageCommon.GetSuccesfully,
                response, Key, value);
        }
        catch (Exception ex)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.GetFailed, ex.Message, Key, value);
        }
    }
}
