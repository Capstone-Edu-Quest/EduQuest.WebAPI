﻿using EduQuest_Domain.Models.Response;
using System.Net;
using System.Text.Json;
using System.Text;
using EduQuest_Domain.Entities;

namespace EduQuest_Application.Helper;

public class GeneralHelper
{
    private static Random random = new Random();
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
    public static string ArrayToString(object[] input)
    {
        StringBuilder valuesBuilder = new StringBuilder();

        foreach (var value in input)
        {
            if (value is JsonElement jsonElement)
            {
                if (jsonElement.ValueKind == JsonValueKind.Number && jsonElement.TryGetInt32(out int intValue))
                {
                    valuesBuilder.Append(intValue);
                }
                else if (jsonElement.ValueKind == JsonValueKind.String)
                {
                    valuesBuilder.Append(jsonElement.GetString());
                }
            }
            else if (value is int intValue)
            {
                valuesBuilder.Append(intValue);
            }
            else if (value is string stringValue)
            {
                valuesBuilder.Append(stringValue);
            }
            valuesBuilder.Append(",");
        }
        if (valuesBuilder.Length > 0)
        {
            valuesBuilder.Length--;
        }

        return valuesBuilder.ToString();
    }

    public static object[] ToArray(string values)
    {
        string[] temp = values.Split(',');
        object[] result = new object[temp.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            try
            {
                result[i] = Convert.ToInt32(temp[i]);
            }
            catch (Exception)
            {
                result[i] = temp[i];
            }
        }
        return result;
    }


    public static int GenerateItemShards(Tag? courseTag)
    {
        if(courseTag == null)
        {
            return 0;
        }
        int shardRandom = random.Next(1, 5);
        return shardRandom;
    }

    public static int GenerateExpEarned(double? learningTime)
    {
        if(learningTime == null) { return 0; }
        return (int)Math.Ceiling((double)learningTime! * 2);
    }
}
