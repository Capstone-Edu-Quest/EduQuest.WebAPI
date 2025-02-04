
using EduQuest_Domain.Constants;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Infrastructure.Configurations;
using EduQuest_Infrastructure.ExternalServices.Authentication.Setting;
using EduQuest_Infrastructure.ExternalServices.Oauth2.Setting;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;

namespace EduQuest_Infrastructure
{
	public static class AppConfigurationService
    {
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			var test = services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
			services.Configure<GoogleSetting>(configuration.GetSection("GoogleToken"));


			#region DbContext
			services.AddDbContext<ApplicationDbContext>((sp, options) =>
			{
				options.UseSqlServer(
					//configuration.GetConnectionString("local"),
					configuration.GetConnectionString("production"),
					b =>
					{
						b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
						b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
					});
				//options.EnableSensitiveDataLogging();
				//options.EnableDetailedErrors();
				options.UseLazyLoadingProxies();
			});

			//services.AddTransient<ApplicationDbContext>();
			//services.AddScoped<IApplicationDbContext>(
				//provider => provider.GetService<ApplicationDbContext>());

			#endregion

			#region Redis
			var redisConnection = configuration["Redis:HostName"];
			var redisDatabase = ConnectionMultiplexer.Connect($"{redisConnection},abortConnect=false");
			services.AddSingleton<IConnectionMultiplexer>(sp =>
			{
				return redisDatabase;
			});

			services.AddStackExchangeRedisCache(options =>
			{
				options.Configuration = redisConnection;
			});
			services.AddDistributedMemoryCache();

			//services.AddSingleton<IDistributedLock>(_ =>
			//new RedisDistributedSynchronizationProvider(redisDatabase!.GetDatabase()));

			#endregion

			#region JWTConfig
			services.AddAuthentication(opt =>
			{
				opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidIssuer = configuration.GetSection("JWTSettings:Issuer").Get<string>(),
					ValidAudience = configuration.GetSection("JWTSettings:Audience").Get<string>(),
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWTSettings:Securitykey").Get<string>())),
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					RequireExpirationTime = true,
					ClockSkew = TimeSpan.Zero
				};
			});

			#endregion

			#region SignalR
			services
				.AddSignalR(option =>
				{
					option.EnableDetailedErrors = true;
					option.ClientTimeoutInterval = TimeSpan.FromMinutes(1);
					option.MaximumReceiveMessageSize = 5 * 1024 * 1024; // 5MB
				})
				.AddJsonProtocol(option =>
				{
					option.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
				});
			#endregion

			#region AddSingleton
			services.AddScoped<IUnitOfWork>(provider => (IUnitOfWork)provider.GetRequiredService<ApplicationDbContext>());
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<ICourseRepository, CourseRepository>();
			services.AddScoped<ITagRepository, TagRepository>();
			services.AddScoped<ILearningMaterialRepository, LearningMaterialRepository>();
			services.AddScoped<IStageRepository, StageRepository>();
			services.AddScoped<IFavoriteListRepository, FavoriteListRepository>();
			services.AddScoped<IQuestRepository, QuestRepository>();
			services.AddScoped<IBadgeRepository, BadgeRepository>();
			services.AddScoped<IUserStatisticRepository, UserStatisticRepository>();
			services.AddScoped<ICourseStatisticRepository, CourseStatisticRepository>();
			services.AddScoped<ISystemConfigRepository, SystemConfigRepository>();
			services.AddScoped<ILearningPathRepository, LearningPathRepository>();
			#endregion

			#region Swagger
			services.AddSwaggerGen(swagger =>
			{
				swagger.SwaggerDoc("v1", new() { Title = "Edu_Quest API", Version = $"{Constants.Http.API_VERSION}" });
				swagger.EnableAnnotations();
				swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Please enter a valid token using the Bearer scheme (\"bearer {token}\")",
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT",
					Scheme = "Bearer"
				});
				swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
					{
						{
							new OpenApiSecurityScheme
							{
								Reference = new OpenApiReference
								{
									Type=ReferenceType.SecurityScheme,
									Id="Bearer"
								}
							},
							new string[]{}
						}
					});

			});

			#endregion

			#region HealthCheck
			var connectionString = configuration.GetSection("Redis:HostName").Get<string>();
			var sqlserver = configuration.GetSection("ConnectionStrings:production").Get<string>();
			//var elasticSearch = configuration.GetSection("ElasticSearch:Url").Get<string>();
			services.AddHealthChecks()
					.AddRedis(connectionString!)
					.AddSqlServer(sqlserver!);
					//.AddElasticsearch(elasticSearch!);
			#endregion

			#region CORS
			services.AddCors(p => p.AddPolicy(Constants.Http.CORS, build =>
			{
				build.WithOrigins("*")
					 .AllowAnyMethod()
					 .AllowAnyHeader();
			}));
			#endregion

			//#region Firebase
			//FirebaseApp.Create(new AppOptions
			//{
			//	Credential = GoogleCredential.FromFile("path/to/service-account.json")
			//});
			//#endregion

			return services;
		}

		#region Serilog
		public static WebApplicationBuilder UseSerilog(this WebApplicationBuilder builder, IConfiguration configuration)
		{
			builder.Host.UseSerilog((cntxt, loggerConfiguration) =>
			{
				loggerConfiguration.ReadFrom.Configuration(configuration);
			});
			return builder;
		}

		public static WebApplication UseLoggingInterceptor(this WebApplication app)
		{
			app.UseSerilogRequestLogging(options =>
			{
				options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Information;
				options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
				{
					diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
					diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
				};
			});
			return app;
		}

		#endregion

		#region Migration
		public static async Task ApplyMigrations(this IServiceProvider serviceProvider)
		{
			using var scope = serviceProvider.CreateScope();
			await using ApplicationDbContext dbContext =
				scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			await dbContext.Database.MigrateAsync();
			//await DatabaseInitializer.InitializeAsync(dbContext);
		}
		public static async Task ManageDataAsync(IServiceProvider svcProvider)
		{
			//Service: An instance of db context
			var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();

			//Migration: This is the programmatic equivalent to Update-Database
			await dbContextSvc.Database.MigrateAsync();
		}
		#endregion




	}
}
