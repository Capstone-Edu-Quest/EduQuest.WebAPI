﻿using AutoMapper;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System.Net;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Courses.Command.CreateCourse
{
	public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, APIResponse>
	{
		private readonly ICourseRepository _courseRepository;	
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserRepository _userRepository;
		private readonly IUserMetaRepository _userMetaRepository;
		private readonly ITagRepository _tagRepository;

		public CreateCourseCommandHandler(ICourseRepository courseRepository, IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository, IUserMetaRepository userMetaRepository, ITagRepository tagRepository)
		{
			_courseRepository = courseRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_userRepository = userRepository;
			_userMetaRepository = userMetaRepository;
			_tagRepository = tagRepository;
		}

		public async Task<APIResponse> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
		{
			var course = _mapper.Map<Course>(request.CourseRequest);
			var listTag = await _tagRepository.GetByIdsAsync(request.CourseRequest.TagIds);
			course.Requirement = ContentHelper.JoinStrings(request.CourseRequest.RequirementList, '.');
			
			var user = await _userRepository.GetById(request.UserId);
			
			course.CreatedBy = user!.Id;
			course.Id = Guid.NewGuid().ToString();
			course.Version = 1;
			//course.LastUpdated = DateTime.Now.ToUniversalTime();
			course.Tags = listTag;
			course.Status = GeneralEnums.StatusCourse.Draft.ToString();
			course.CourseStatistic = new CourseStatistic
			{
				Id = Guid.NewGuid().ToString(),
				CourseId = course.Id,
				TotalLearner = 0,
				TotalLesson = 0,
				TotalReview = 0,
				Rating = 0,
				TotalTime = 0,
				CreatedAt = DateTime.Now.ToUniversalTime(),
				UpdatedAt = DateTime.Now.ToUniversalTime(),
			};
			await _courseRepository.Add(course);
			
			//User Meta
			var userMeta = await _userMetaRepository.GetByUserId(user.Id);
			userMeta!.TotalCourseCreated++;
			await _userMetaRepository.Update(userMeta);

			var result = await _unitOfWork.SaveChangesAsync() > 0;
			return new APIResponse
			{
				IsError = !result,
				Payload = result ? course : null,
				Errors = result ? null : new ErrorResponse
				{
					StatusResponse = HttpStatusCode.BadRequest,
					StatusCode = (int)HttpStatusCode.BadRequest,
					Message = MessageCommon.SavingFailed,
				},
				Message = new MessageResponse
				{
					content = result ? MessageCommon.CreateSuccesfully : MessageCommon.CreateFailed,
					values = new Dictionary<string, string> { { "name", "course" } }
				}
			};


		}
	}
}
