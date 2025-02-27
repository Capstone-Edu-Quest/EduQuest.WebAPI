using EduQuest_Application.Abstractions.Redis;
using EduQuest_Domain.Models.Response;
using System.Net;

public class TokenBlacklistMiddleware : IMiddleware
{
    private readonly IRedisCaching _redisDatabase;

    public TokenBlacklistMiddleware(IRedisCaching redisDatabase)
    {
        _redisDatabase = redisDatabase;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {

        if (context.Request.Headers.TryGetValue("Authorization", out var token))
        {
            var accessToken = token.ToString().Replace("Bearer ", string.Empty);

            var isBlacklisted = await _redisDatabase.GetAsync<string>($"Token_{accessToken}");

            if (isBlacklisted != null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsJsonAsync(new APIResponse
                {
                    IsError = true,
                    Errors = new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                        Message = EduQuest_Domain.Constants.Constants.MessageCommon.TokenBlackListed,
                    },
                });
                return;
            }
        }

        await next(context);
    }
}
