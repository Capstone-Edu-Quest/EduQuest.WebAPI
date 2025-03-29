using EduQuest_Domain.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.UseCases.FavoriteCourse.Query.GetUserFavoriteList;

public class GetUserFavoriteListQuery: IRequest<APIResponse>
{
    public string UserId { get; set; }
    public string? Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public string? Requirement { get; set; }
    public string? Feature { get; set; }
    public int Page { get; set; }
    public int EachPage { get; set; }

    public GetUserFavoriteListQuery(string userId, string? title, string? description, 
        decimal? price, string? requirement, string? feature, int page, int eachPage)
    {
        UserId = userId;
        Title = title;
        Description = description;
        Price = price;
        Requirement = requirement;
        Feature = feature;
        Page = page;
        EachPage = eachPage;
    }
}
