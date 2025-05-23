﻿using AutoMapper;
using EduQuest_Application.Mappings;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.DTO.Response.Profiles;

public class LeanerProfileStatisticDto : IMapFrom<UserMeta>, IMapTo<UserMeta>
{
    public long? Rank { get; set; } = 0;
    public int? LongestStreak { get; set; } = 0;
    public double? TotalLearningTime { get; set; } = 0;
    public int? TotalLearningCourses { get; set; }
    public int? FavoriteTopics { get; set; }
    public void MappingFrom(Profile profile)
    {
        profile.CreateMap<UserMeta, LeanerProfileStatisticDto>()
            .ForMember(dest => dest.TotalLearningTime, opt => opt.MapFrom(src => src.TotalStudyTime));
    }

}
