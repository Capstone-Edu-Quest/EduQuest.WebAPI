using EduQuest_API.Middleware;
using EduQuest_Infrastructure;
using EduQuest_Application;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.
builder.ConfigureSerilog();

builder.Services.AddScoped<GlobalException>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();


#region Add configurations to Services
{
	services.AddApplication(builder.Configuration);
	services.AddInfrastructure(builder.Configuration);
	builder.UseSerilog(builder.Configuration);
}
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	await app.Services.ApplyMigrations();
	
}
var scope = app.Services.CreateScope();
await AppConfigurationService.ManageDataAsync(scope.ServiceProvider);
app.UseMiddleware<CorsMiddleware>();
app.UseLoggingInterceptor();
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/h", new HealthCheckOptions
{
	Predicate = _ => true,
	ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.ConfigureExceptionHandler();
app.Run();
