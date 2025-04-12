using AutoMapper;
using EduQuest_Application.DTO.Response.UserStatistics;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository;

namespace EduQuest_Application.Mappings;

public class MaxExpLevelResolver : IValueResolver<UserMeta, UserStatisticDto, int?>
{
    private readonly ILevelRepository _levelRepository;

    public MaxExpLevelResolver(ILevelRepository levelRepository)
    {
        _levelRepository = levelRepository;
    }

    public int? Resolve(UserMeta source, UserStatisticDto destination, int? destMember, ResolutionContext context)
    {
        if (source.Level == null) return null;
        var level = _levelRepository.GetExpByLevel(source.Level.Value);

        return level;
    }
}
