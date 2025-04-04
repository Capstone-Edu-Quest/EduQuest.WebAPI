

using AutoMapper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using EduQuest_Domain.Entities;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.Helper;
using static EduQuest_Domain.Enums.GeneralEnums;

namespace EduQuest_Application.UseCases.LearningPaths.Commands.UpdateLearningPath;

public class UpdateLearningPathHandler : IRequestHandler<UpdateLearningPathCommand, APIResponse>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly ICourseStatisticRepository _courseStatisticRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICourseRepository _courseRepository;
    private const string Key = "name";
    private const string value = "learning path";
    public UpdateLearningPathHandler(ILearningPathRepository learningPathRepository, 
        IUserRepository userRepository,
        IMapper mapper, IUnitOfWork unitOfWork, 
        ICourseStatisticRepository courseStatisticRepository, ICourseRepository courseRepository)
    {
        _learningPathRepository = learningPathRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _courseStatisticRepository = courseStatisticRepository;
        _userRepository = userRepository;
        _courseRepository = courseRepository;
    }

    public async Task<APIResponse> Handle(UpdateLearningPathCommand request, CancellationToken cancellationToken)
    {
        try
        {
            #region validate
            //validate Learing path exist 
            var learingPath = await _learningPathRepository.GetLearningPathDetail(request.LearningPathId);
            if (learingPath == null)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed, MessageCommon.NotFound, Key, value);
            }
            //validate owner
            User? user = await _userRepository.GetById(request.UserId);

            if (user == null)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed, MessageCommon.UserDontHavePer, Key, value);
            }

            bool isOwner = await _learningPathRepository.IsOwner(request.UserId, request.LearningPathId);
            bool isExpert = int.TryParse(user.RoleId, out int roleId) && roleId == (int)UserRole.Expert;
            bool createdByExpert = learingPath.CreatedByExpert == true;

            if (!isOwner && !(isExpert && createdByExpert))
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed, MessageCommon.UserDontHavePer, Key, value);
            }
            #endregion

            learingPath.Name = !string.IsNullOrEmpty(request.LearningPathRequest.Name) ? request.LearningPathRequest.Name : learingPath.Name;
            learingPath.Description = !string.IsNullOrEmpty(request.LearningPathRequest.Description) ? request.LearningPathRequest.Description : learingPath.Description;
            if (request.LearningPathRequest.IsPublic.HasValue)
            {
                learingPath.IsPublic = request.LearningPathRequest.IsPublic.Value;
            }
            learingPath.UpdatedAt = DateTime.Now.ToUniversalTime();
            learingPath.UpdatedBy = request.UserId;

            //handler learning path courses
            var courses = learingPath.LearningPathCourses;
            int Flag = 100;
            foreach (var updatecourse in request.LearningPathRequest.Courses)
            {
                //add new courses: new courses get to the bottom of the courses order
                if (updatecourse.Action == "add")
                {
                    updatecourse.CourseOrder = courses.Count;
                    bool isExist = courses.Any(c => c.CourseId == updatecourse.CourseId);
                    if (isExist)
                    {
                        return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed, MessageCommon.AlreadyExists, Key, value);
                    }
                    LearningPathCourse temp = courses.FirstOrDefault(c => c.CourseOrder == updatecourse.CourseOrder)!;
                    if (temp != null) updatecourse.CourseOrder += Flag;
                    learingPath.LearningPathCourses.Add(_mapper.Map<LearningPathCourse>(updatecourse));
                    var cs = await _courseStatisticRepository.GetByCourseId(updatecourse.CourseId);
                    learingPath.TotalTimes += cs.TotalTime!.Value;
                    Flag += 1;
                }
                if (updatecourse.Action == "delete")
                {
                    LearningPathCourse? temp = courses.FirstOrDefault(c => c.CourseId == updatecourse.CourseId);
                    if(temp == null)
                    {
                        return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed, MessageCommon.NotFound, Key, value);
                    }
                    learingPath.LearningPathCourses.Remove(temp);
                    var cs = await _courseStatisticRepository.GetByCourseId(updatecourse.CourseId);
                    learingPath.TotalTimes -= cs.TotalTime!.Value;
                }

                //update course order/position
                if (updatecourse.Action == "update")
                {
                    LearningPathCourse temp = courses.FirstOrDefault(c => c.CourseId == updatecourse.CourseId)!;
                    temp.CourseOrder = updatecourse.CourseOrder;
                }
            }
            List<string> courseIds = new List<string>();
            //rearrange course order after update, some course might have abnormal order number, 
            //this code will rearrange the order based on the old order number
            //ex: after update course1 have order 2, course2 have order 8, after the rearrangement, the order will be come 1,2...
            var orderedCourses = learingPath.LearningPathCourses
            .OrderBy(c => c.CourseOrder)
            .ToList();
            for (int i = 0; i < orderedCourses.Count; i++)
            {
                orderedCourses[i].CourseOrder = i;
                courseIds.Add(orderedCourses[i].CourseId);
            }

            List<Tag> tags = await _courseRepository.GetTagByCourseIds(courseIds);
            //post update learning path tags:
            var currentTags = learingPath.Tags.ToList();

            // new tags (exist in tags but not in currentTags)
            var tagsToAdd = tags.Where(nt => !currentTags.Any(ct => ct.Id == nt.Id)).ToList();

            // deleted tag (exist in currentTags but not in tags)
            var tagsToRemove = currentTags.Where(ct => tags.Any(nt => nt.Id == ct.Id)).ToList();

            // update learning path tags
            foreach (var tag in tagsToAdd)
            {
                learingPath.Tags.Add(tag);
            }

            foreach (var tag in tagsToRemove)
            {
                learingPath.Tags.Remove(tag);
            }
            await _learningPathRepository.Update(learingPath);
            if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                
                CommonUserResponse userResponse = _mapper.Map<CommonUserResponse>(user);
                MyLearningPathResponse myLearningPathResponse = _mapper.Map<MyLearningPathResponse>(learingPath);
                myLearningPathResponse.TotalCourses = learingPath.LearningPathCourses.Count;
                myLearningPathResponse.CreatedBy = userResponse;
                return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.UpdateSuccesfully,
                    myLearningPathResponse, Key, value);
            }
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed, MessageCommon.UpdateFailed, Key, value);
        }
        catch (Exception ex)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed, ex.Message, Key, value);
        }
    }
}
