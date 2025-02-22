using Serilog;

namespace EduQuest_API.Middleware
{
    public static class LogConfiguration
    {
        public static void ConfigureSerilog(this WebApplicationBuilder builder)
        {

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}
