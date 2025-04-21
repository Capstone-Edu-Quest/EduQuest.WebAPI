using AutoMapper;
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Application.DTO.Response.Users;
using EduQuest_Application.Helper;
using EduQuest_Domain.Entities;
using EduQuest_Domain.Models.Response;
using EduQuest_Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EduQuest_Domain.Constants.Constants;

namespace EduQuest_Application.UseCases.UserMetas.Queries.GetLeaderboard;

public class GetLeaderboardHandler : IRequestHandler<GetLeaderboardQuery, APIResponse>
{
    private readonly IRedisCaching _redis;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public GetLeaderboardHandler(IRedisCaching redis, IMapper mapper, IUserRepository userRepository)
    {
        _redis = redis;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<APIResponse> Handle(GetLeaderboardQuery request, CancellationToken cancellationToken)
    {
        List<(string member, double score, long rank)> result = await _redis.GetTopSortedSetAsync("leaderboard:season1", 100);
        List<String> ids = result.Select(r => r.member).ToList();
        List<User>? users = await _userRepository.GetByUserIds(ids);
        List<UserRankingResponse> response = new List<UserRankingResponse>();
        if(users != null && users.Count > 0)
        {
            foreach(User user in users)
            {
                UserRankingResponse ranking = _mapper.Map<UserRankingResponse>(user);
                var rank = result.Where(r => r.member == ranking.Id).FirstOrDefault();
                ranking.score = rank.score;
                ranking.rank = rank.rank;
                response.Add(ranking);
            }
        }
        response = response.Where(r => r.score > 0).OrderBy(r => r.rank).ToList();
        return GeneralHelper.CreateSuccessResponse(System.Net.HttpStatusCode.OK, MessageCommon.GetSuccesfully,
                response, "name", "leaderboard");
    }
}
