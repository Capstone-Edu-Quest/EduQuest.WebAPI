using AutoMapper;
using EduQuest_Application.DTO.Response.Courses;
using EduQuest_Application.DTO.Response.UserStatistics;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response.Carts
{
	public class MyCartReponse : IMapFrom<Cart>, IMapTo<Cart>
	{
        public string Id { get; set; }
        public decimal? Total { get; set; }
        public int? NumOfCourse { get; set; }
        public bool? isFinished { get; set; }
        public string? Url { get; set; }
        public List<CourseSearchResponse>? Courses { get; set; }

        public void MappingFrom(Profile profile)
        {
			profile.CreateMap<Cart, MyCartReponse>()
			   .ForMember(dest => dest.isFinished, opt => opt.MapFrom(
				   src => !src.User.Transactions.Any(t => t.Type == "CheckoutCart" && t.Status == "Pending")
			   ))
			   .ForMember(dest => dest.Url, opt => opt.MapFrom(
					src => src.User.Transactions
					.FirstOrDefault(t => t.Type == "CheckoutCart" && t.Status == "Pending") != null
					? src.User.Transactions
					.FirstOrDefault(t => t.Type == "CheckoutCart" && t.Status == "Pending").Url
					: null
				));


		}
    }
}
