using AutoMapper;
using EduQuest_Application.DTO.Response.Carts;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.Helper;
using EduQuest_Domain.Enums;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.Carts.Query
{
	public class GetCartByUserIdQueryHandler : IRequestHandler<GetCartByUserIdQuery, APIResponse>
	{
		private readonly ICartRepository _cartRepository;
		private readonly ICourseRepository _courseRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICourseStatisticRepository _courseStatisticRepository;
		private readonly IUserRepository _userRepository;

		public GetCartByUserIdQueryHandler(ICartRepository cartRepository, ICourseRepository courseRepository, IMapper mapper, IUnitOfWork unitOfWork, ICourseStatisticRepository courseStatisticRepository, IUserRepository userRepository)
		{
			_cartRepository = cartRepository;
			_courseRepository = courseRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_courseStatisticRepository = courseStatisticRepository;
			_userRepository = userRepository;
		}

		public async Task<APIResponse> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
		{
			var apiResponse = new APIResponse();
			var cart = await _cartRepository.GetByUserId(request.UserId);

			var cartResponse = _mapper.Map<MyCartReponse>(cart);
			var listCourseId = cart.CartItems.Select(c => c.CourseId).Distinct().ToList();
			var listCourse = await _courseRepository.GetByListIds(listCourseId);

			var listCourseResponse = _mapper.Map<List<CourseSearchResponse>>(listCourse);
			cartResponse.NumOfCourse = listCourse.Count();
			foreach (var course in listCourseResponse)
			{
				var user = await _userRepository.GetById(course.CreatedBy);
				course.Author = user!.Username!;

				var courseSta = await _courseStatisticRepository.GetByCourseId(course.Id);
				if (courseSta != null)
				{
					course.TotalLesson = (int)courseSta.TotalLesson;
					course.TotalReview = (int)courseSta.TotalReview;
					course.Rating = (int)courseSta.Rating;
					course.TotalTime = (int)courseSta.TotalTime;
				}
			}
			cartResponse.Courses = listCourseResponse;
			return apiResponse = GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, cartResponse, "name", "cart");

		}
	}
}
