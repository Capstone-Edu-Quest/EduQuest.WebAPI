using EduQuest_API.Middleware;
using EduQuest_Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


#region Add configurations to Services
{
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
app.Run();
