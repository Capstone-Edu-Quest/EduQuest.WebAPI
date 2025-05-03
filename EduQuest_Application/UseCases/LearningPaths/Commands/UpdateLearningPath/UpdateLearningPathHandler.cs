

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
using Stripe;

namespace EduQuest_Application.UseCases.LearningPaths.Commands.UpdateLearningPath;

public class UpdateLearningPathHandler : IRequestHandler<UpdateLearningPathCommand, APIResponse>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly ICourseStatisticRepository _courseStatisticRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICourseRepository _courseRepository;
    private readonly IEnrollerRepository _enrollerRepository;
    private const string Key = "name";
    private const string value = "learning path";
    public UpdateLearningPathHandler(ILearningPathRepository learningPathRepository,
        IUserRepository userRepository,
        IMapper mapper, IUnitOfWork unitOfWork, IEnrollerRepository enrollerRepository,
        ICourseStatisticRepository courseStatisticRepository, ICourseRepository courseRepository)
    {
        _learningPathRepository = learningPathRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _courseStatisticRepository = courseStatisticRepository;
        _userRepository = userRepository;
        _courseRepository = courseRepository;
        _enrollerRepository = enrollerRepository;
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
            List<Enroller> addedCourses = new List<Enroller>();
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
                //delete course
                if (updatecourse.Action == "delete")
                {
                    LearningPathCourse? temp = courses.FirstOrDefault(c => c.CourseId == updatecourse.CourseId);
                    if (temp == null)
                    {
                        continue;
                        /*return GeneralHelper.CreateErrorResponse(HttpStatusCode.NotFound, MessageCommon.UpdateFailed, MessageCommon.NotFound, Key, value);*/
                    }
                    var enrollers = await _enrollerRepository.GetByCourseId(learingPath.Id, updatecourse.CourseId);
                    if (enrollers != null)
                    {
                        _enrollerRepository.DeleteRange(enrollers);
                    }
                    learingPath.LearningPathCourses.Remove(temp);
                    var cs = await _courseStatisticRepository.GetByCourseId(updatecourse.CourseId);
                    learingPath.TotalTimes -= cs.TotalTime!.Value;
                    await _unitOfWork.SaveChangesAsync();
                }

                //update course order/position
                if (updatecourse.Action == "update")
                {
                    LearningPathCourse temp = courses.FirstOrDefault(c => c.CourseId == updatecourse.CourseId)!;
                    temp.CourseOrder = updatecourse.CourseOrder;
                    var updateEnrollers = await _enrollerRepository.GetByCourseId(learingPath.Id, updatecourse.CourseId);
                    if (updateEnrollers != null)
                    {
                        foreach (var item in updateEnrollers)
                        {
                            item.CourseOrder = updatecourse.CourseOrder;
                        }
                        await _enrollerRepository.UpdateRangeAsync(updateEnrollers);
                    }
                    await _unitOfWork.SaveChangesAsync();
                }
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
                    var Ids = await _enrollerRepository.GetEnrollerIds(learingPath.Id);

                    if (Ids != null)
                    {
                        foreach (var item in Ids)
                        {
                            Enroller newEnroller = new Enroller();
                            newEnroller.CourseId = updatecourse.CourseId;
                            newEnroller.LearningPathId = learingPath.Id;
                            newEnroller.UserId = item.UserId;
                            newEnroller.EnrollDate = item.EnrollDate;
                            newEnroller.Id = Guid.NewGuid().ToString();
                            newEnroller.CreatedAt = DateTime.Now.ToUniversalTime();
                            newEnroller.EnrollDate = item.EnrollDate!.Value.ToUniversalTime();
                            newEnroller.IsOverDue = false;
                            newEnroller.CourseOrder = updatecourse.CourseOrder;
                            int addedDate = await _enrollerRepository.GetTotalLearningDay(learingPath.Id, item.UserId);
                            int courseLearingDate = GetLearningDate(cs.TotalTime);
                            addedDate += courseLearingDate;
                            newEnroller.DueDate = item.DueDate!.Value.AddDays(addedDate).ToUniversalTime();
                            var courseData = await _courseRepository.GetById(updatecourse.CourseId);
                            var learner = courseData.CourseLearners!.FirstOrDefault(c => c.UserId == request.UserId);
                            if (learner != null && learner.ProgressPercentage >= 100)
                            {
                                newEnroller.IsCompleted = true;
                            }
                            addedCourses.Add(newEnroller);
                        }
                    }
                    Flag += 1;
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
                var temp = addedCourses.FirstOrDefault(e => e.CourseId == orderedCourses[i].CourseId);
                if (temp != null)
                {
                    temp.CourseOrder = i;
                }
                courseIds.Add(orderedCourses[i].CourseId);
            }

            List<Tag> tags = await _courseRepository.GetTagByCourseIds(courseIds);
            //post update learning path tags:
            var currentTags = learingPath.Tags.ToList();

            // new tags (exist in tags but not in currentTags)
            var tagsToAdd = tags.Where(nt => !currentTags.Any(ct => ct.Id == nt.Id)).ToList();

            // deleted tag (exist in currentTags but not in tags)
            var tagsToRemove = currentTags.Where(ct => !tags.Any(nt => nt.Id == ct.Id)).ToList();

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
            await _enrollerRepository.CreateRangeAsync(addedCourses);
            await _unitOfWork.SaveChangesAsync();

            CommonUserResponse userResponse = _mapper.Map<CommonUserResponse>(user);
            MyLearningPathResponse myLearningPathResponse = _mapper.Map<MyLearningPathResponse>(learingPath);
            myLearningPathResponse.TotalCourses = learingPath.LearningPathCourses.Count;
            myLearningPathResponse.CreatedBy = userResponse;
            return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.UpdateSuccesfully,
                myLearningPathResponse, Key, value);
        }
        catch (Exception ex)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed, ex.Message, Key, value);
        }
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
