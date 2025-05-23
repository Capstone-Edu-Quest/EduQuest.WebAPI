﻿using AutoMapper;
using EduQuest_Application.Abstractions.Firebase;
using EduQuest_Application.DTO.Response.Feedbacks;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Notification;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Feedbacks.Commands.CreateFeedback;

public class CreateFeedbackHandler : IRequestHandler<CreateFeedbackCommand, APIResponse>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILearnerRepository _learnerRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IFireBaseRealtimeService _fireBaseRealtimeService;
    private const string Key = "name";
    private const string value = "feedback";

    public CreateFeedbackHandler(IFeedbackRepository feedbackRepository, IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork, ILearnerRepository learnerRepository, ICourseRepository courseRepository, IFireBaseRealtimeService fireBaseRealtimeService)
    {
        _feedbackRepository = feedbackRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _learnerRepository = learnerRepository;
        _courseRepository = courseRepository;
        _fireBaseRealtimeService = fireBaseRealtimeService;
    }

    public async Task<APIResponse> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // validate user
            var user = await _userRepository.GetById(request.UserId);
            if (user == null)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.Unauthorized, MessageCommon.CreateFailed, MessageCommon.NotFound, Key, value);
            }
            //validate if user have registerd in course
            if(!await _learnerRepository.RegisteredCourse(request.Feedback.CourseId, request.UserId))
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.CreateFailed, MessageCommon.UserDontHavePer, Key, value);
            }
            Feedback newFeedback = _mapper.Map<Feedback>(request.Feedback);
            newFeedback.UserId = request.UserId;
            newFeedback.Id = Guid.NewGuid().ToString();
            await _feedbackRepository.Add(newFeedback);
            await _unitOfWork.SaveChangesAsync();

			//Update Course Statistic
			var courseStatistic = await _courseRepository.GetCourseById(request.Feedback.CourseId);

			var feedbacks = await _feedbackRepository.GetAllByCourseId(request.Feedback.CourseId);
			var averageRating = feedbacks.Any() ? feedbacks.Average(f => f.Rating) : 0;
			courseStatistic.CourseStatistic.Rating = averageRating;
            if(courseStatistic.CourseStatistic.TotalReview != null)
            {
                courseStatistic.CourseStatistic.TotalReview += 1;
            }
            else
            {
                courseStatistic.CourseStatistic.TotalReview = 1;
            }

            await _courseRepository.Update(courseStatistic);

			if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                //await _fireBaseRealtimeService.PushNotificationAsync(new NotificationDto
                //{
                //    userId = courseStatistic.CreatedBy,
                //    Receiver = courseStatistic.CreatedBy,
                //    Content = NotificationMessage.YOUR_COURSE_WAS_RATED,
                //    Url = $"/my-courses/{courseStatistic.Id}",
                //    Values = new Dictionary<string, string>
                //    {
                //        { "name", user.Username },
                //    }
                //});
                return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.CreateSuccesfully,
                    _mapper.Map<FeedbackResponse>(newFeedback), Key, value);
            }

            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.CreateFailed, MessageCommon.CreateFailed, Key, value);

        }catch(Exception ex)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.CreateFailed, ex.Message, Key, value);
        }
    }
}
