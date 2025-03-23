using AutoMapper;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Domain.Repository;
using MediatR;
using static EduQuest_Domain.Constants.Constants;
using System.Net;
using EduQuest_Application.DTO.Response.Feedbacks;
using EduQuest_Application.Helper;

namespace EduQuest_Application.UseCases.Feedbacks.Commands.UpdateFeedback;

internal class UpdateFeedbackHandler : IRequestHandler<UpdateFeedbackCommand, APIResponse>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
	private readonly ICourseRepository _courseRepository;
	private const string Key = "name";
    private const string value = "feedback";

	public UpdateFeedbackHandler(IFeedbackRepository feedbackRepository, IMapper mapper, IUnitOfWork unitOfWork, ICourseRepository courseRepository)
	{
		_feedbackRepository = feedbackRepository;
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_courseRepository = courseRepository;
	}

	public async Task<APIResponse> Handle(UpdateFeedbackCommand request, CancellationToken cancellationToken)
    {
        try
        {
            #region validate owner
            bool isOwner = await _feedbackRepository.IsOnwer(request.FeedbackId, request.UserId);
            if (!isOwner)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed, MessageCommon.UserDontHavePer, Key, value);
            }
            #endregion

            var feedback = await _feedbackRepository.GetById(request.FeedbackId);
            if (feedback == null)
            {
                return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed, MessageCommon.NotFound, Key, value);
            }

            feedback.Rating = request.Feedback.Rating;
            feedback.Comment = request.Feedback.Comment;
            feedback.UpdatedAt = DateTime.Now.ToUniversalTime();
            feedback.UpdatedBy = request.UserId;

            await _feedbackRepository.Update(feedback);
            await _unitOfWork.SaveChangesAsync();

			//Update Course Statistic
			var courseStatistic = await _courseRepository.GetCourseById(feedback.CourseId);

			var feedbacks = await _feedbackRepository.GetByCourseId(feedback.CourseId, 1, 10, null, null);
			var averageRating = feedbacks.Any() ? feedbacks.Average(f => f.Rating) : 0;
			courseStatistic.CourseStatistic.Rating = averageRating;

			await _courseRepository.Update(courseStatistic);

			#region return value
			if (await _unitOfWork.SaveChangesAsync() > 0)
            {
                return GeneralHelper.CreateSuccessResponse(HttpStatusCode.OK, MessageCommon.UpdateSuccesfully,
                    _mapper.Map<FeedbackResponse>(feedback), Key, value);
            }
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed, MessageCommon.UpdateFailed, Key, value);
            #endregion
        }
        catch(Exception ex)
        {
            return GeneralHelper.CreateErrorResponse(HttpStatusCode.BadRequest, MessageCommon.UpdateFailed, ex.Message, Key, value);
        }
    }
}
