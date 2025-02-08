

using AutoMapper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.UseCases.LearningPaths.Queries.GetLearningPathDetail;

public class GetLearningPathDetailHandler : IRequestHandler<GetLearningPathDetailQuery, APIResponse>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly IMapper _mapper;

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
                        content = MessageCommon.NotFound,
                        values = new Dictionary<string, string> { { "name", "learning path" } }
                    }
                };
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
                return learningPathCourse;
            }).ToList();

            // parse learningPathCourses, CreatedBy and TotalCourses
            response.TotalCourses = learningPathCourses.Count;
            response.Courses = learningPathCourses.OrderBy(r => r.Order).ToList();
            response.CreatedBy = _mapper.Map<CommonUserResponse>(learningPath.User);
            return new APIResponse
            {
                IsError = false,
                Payload = response,
                Errors = null,
                Message = new MessageResponse
                {
                    content = MessageCommon.GetSuccesfully,
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
                    content = MessageCommon.GetFailed,
                    values = new Dictionary<string, string> { { "name", "learning path" } }
                }
            };
        }
    }
}
