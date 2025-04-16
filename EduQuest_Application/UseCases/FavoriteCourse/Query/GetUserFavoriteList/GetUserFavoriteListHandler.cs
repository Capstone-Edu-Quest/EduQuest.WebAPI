using AutoMapper;
using EduQuest_Application.DTO.Response.Coupons;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.DTO.Response.LearningPaths;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Pagination;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.FavoriteCourse.Query.GetUserFavoriteList;

public class GetUserFavoriteListHandler : IRequestHandler<GetUserFavoriteListQuery, APIResponse>
{
    private readonly IFavoriteListRepository _favoriteListRepository;
    private readonly IMapper _mapper;
	private readonly IUserRepository _userRepository;

	public GetUserFavoriteListHandler(IFavoriteListRepository favoriteListRepository, IMapper mapper, IUserRepository userRepository)
	{
		_favoriteListRepository = favoriteListRepository;
		_mapper = mapper;
		_userRepository = userRepository;
	}

	public async Task<APIResponse> Handle(GetUserFavoriteListQuery request, CancellationToken cancellationToken)
    {
        var listCourses = await _favoriteListRepository.GetFavoriteListByUserId(request.UserId);
       

		var temp = listCourses.Courses!.AsQueryable();
        #region Filter
        if (!string.IsNullOrEmpty(request.Title))
        {
            temp = temp.Where(c => c.Title.Contains(request.Title));
        }
        if (!string.IsNullOrEmpty(request.Description))
        {
            temp = temp.Where(c => c.Description.Contains(request.Description));
        }
        if (request.Price.HasValue)
        {
            temp = temp.Where(c => c.Price >= request.Price);
        }
        if (!string.IsNullOrEmpty(request.Requirement))
        {
            temp = temp.Where(c => c.Requirement.Contains(request.Requirement));
        }
       
        temp = temp.Skip((request.Page - 1) * request.EachPage)
                   .Take(request.EachPage);
        #endregion
        List<Course> courses = temp.ToList();
        List<OverviewCourseResponse> responseDto = new List<OverviewCourseResponse>();
        foreach (var item in courses)
        {
            //CommonUserResponse userResponse = _mapper.Map<CommonUserResponse>(item.User);
            OverviewCourseResponse myFavCourseResponse = _mapper.Map<OverviewCourseResponse>(item);
			myFavCourseResponse.RequirementList = ContentHelper.SplitString(item.Requirement, '.');
			var user = await _userRepository.GetById(item.CreatedBy);
			myFavCourseResponse.Author = user!.Username!;
			responseDto.Add(myFavCourseResponse);
        }
        PagedList<OverviewCourseResponse> responses = new PagedList<OverviewCourseResponse>(responseDto, responseDto.Count, request.Page,request.EachPage);
        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully, responses,"name", "favorite courses");
    }
}
