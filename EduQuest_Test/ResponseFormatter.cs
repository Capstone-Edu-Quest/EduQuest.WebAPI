using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace EduQuest_Test;


public static class ResponseFormatter
{
    public static async Task<string> FormatResponse(HttpResponseMessage response, ITestOutputHelper outputHelper)
    {
        var responseBody = await response.Content.ReadAsStringAsync();

        var formattedJson = JsonSerializer.Serialize(
            JsonSerializer.Deserialize<object>(responseBody),
            new JsonSerializerOptions { WriteIndented = true }
        );

        outputHelper.WriteLine($"Response:\n{formattedJson}");

        return formattedJson; 
    }
}
