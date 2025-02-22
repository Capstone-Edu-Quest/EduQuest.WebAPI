using EduQuest_Domain.Models.Response;
using System.Net;

namespace EduQuest_Application.Helper;

public class GeneralHelper
{
    public static APIResponse CreateErrorResponse(HttpStatusCode statusCode, string message, 
        string messageError, string key, string value)
    {
        return new APIResponse
        {
            IsError = true,
            Payload = null,
            Errors = new ErrorResponse
            {
                StatusCode = (int)statusCode,
                StatusResponse = statusCode,
                Message = messageError
            },
            Message = new MessageResponse
            {
                content = message,
                values = new Dictionary<string, string> { { key, value } }
            }
        };
    }


    public static APIResponse CreateSuccessResponse(HttpStatusCode statusCode, string message,
        object data, string key, string value)
    {
        return new APIResponse
        {
            IsError = false,
            Payload = data,
            Errors = null,
            Message = new MessageResponse
            {
                content = message,
                values = new Dictionary<string, string> { { key, value } }
            }
        };
    }
}
