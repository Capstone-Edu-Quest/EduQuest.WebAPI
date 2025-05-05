using AutoMapper;
using EduQuest_Application.DTO.Response.Tags;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Application.DTO.Response.Users
{
    public class UserBasicResponseDto : IMapFrom<User>, IMapTo<User>
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? ExpertiseTagId { get; set; }
        public string? ExpertiseTag { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string Status { get; set; } = null!;
        public string Headline { get; set; }
        public string Description { get; set; }
        public string AvatarUrl { get; set; }
        public string RoleId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<UserTagDto> Tags { get; set; }


        public void MappingFrom(Profile profile)
        {
            profile.CreateMap<User, UserBasicResponseDto>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src =>
                src.UserTags.Where(ut => ut.Tag != null)
                    .Select(ut => new UserTagDto
                    {
                        TagId = ut.Tag!.Id,
                        TagName = ut.Tag.Name
                    })
            ));

        }
    }
}
