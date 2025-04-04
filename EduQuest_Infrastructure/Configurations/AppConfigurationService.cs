
using EduQuest_Application.Abstractions.Authentication;
using EduQuest_Application.Abstractions.Firebase;
using EduQuest_Application.Abstractions.Oauth2;
using EduQuest_Domain.Constants;
using EduQuest_Domain.Repository;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Infrastructure.ExternalServices.Authentication;
using EduQuest_Infrastructure.ExternalServices.Authentication.Setting;
using EduQuest_Infrastructure.ExternalServices.Firebase;
using EduQuest_Infrastructure.ExternalServices.Oauth2.Setting;
using EduQuest_Infrastructure.Persistence;
using EduQuest_Infrastructure.Repository;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore.V1;
using Google.Cloud.Firestore;
using Infrastructure.ExternalServices.Oauth2;
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
using EduQuest_Application.Abstractions.Redis;
using EduQuest_Infrastructure.ExternalServices.Redis;
using EduQuest_Domain.Repository.Generic;
using EduQuest_Infrastructure.Repository.Generic;
using EduQuest_Domain.Models.Payment;
using Stripe;
using EduQuest_Application.ExternalServices.QuartzService;
using EduQuest_Infrastructure.ExternalServices.Quartz;
using Quartz;
using EduQuest_Infrastructure.ExternalServices.Quartz.Quests;
using EduQuest_Application.Helper;
using Infrastructure.ExternalServices.Email.Setting;
using EduQuest_Application.Abstractions.Email;
using EduQuest_Infrastructure.ExternalServices.Email;

namespace EduQuest_Infrastructure
{
    public static class AppConfigurationService
    {
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
			services.Configure<GoogleSetting>(configuration.GetSection("GoogleToken"));
			services.Configure<StripeModel>(configuration.GetSection("Stripe"));
			services.Configure<EmailSetting>(configuration.GetSection("SmtpSettings"));


			#region DbContext
			services.AddDbContext<ApplicationDbContext>((sp, options) =>
			{
				options.UseNpgsql(
					//configuration.GetConnectionString("test"),
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
            services.AddSingleton<IRedisCaching, RedisCaching>();
            services.AddScoped<IUnitOfWork>(provider => (IUnitOfWork)provider.GetRequiredService<ApplicationDbContext>());
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<ICourseRepository, CourseRepository>();
			services.AddScoped<ITagRepository, TagRepository>();
			services.AddScoped<IMaterialRepository, MaterialRepository>();
			services.AddScoped<ILessonRepository, LessonRepository>();
			services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<ITokenValidation, TokenValidation>();
            services.AddScoped<IQuestRepository, QuestRepository>();
            services.AddScoped<IFavoriteListRepository, FavoriteListRepository>();
            services.AddScoped<ITransactionDetailRepository, TransactionDetailRepository>();
            services.AddScoped<ICourseStatisticRepository, CourseStatisticRepository>();
            services.AddScoped<IUserMetaRepository, UserMetaRepository>();
            services.AddScoped<ISystemConfigRepository, SystemConfigRepository>();
			services.AddScoped<IFirebaseMessagingService, FirebaseMessagingService>();
			services.AddScoped<IFirebaseFirestoreService, FirebaseFirestoreService>();
			services.AddScoped<IUserMetaRepository, UserMetaRepository>();
			services.AddScoped<ILearningPathRepository, LearningPathRepository>();
			services.AddScoped<IShopItemRepository, ShopItemRepository>();
			services.AddScoped<IMascotInventoryRepository, MascotInventoryRepository>();
			services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<IEmailService, EmailServices>();

            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
			services.AddScoped<ITransactionRepository, TransactionRepository>();
			
			services.AddScoped<ICartRepository, CartRepository>();
			services.AddScoped<AccountService>();
			services.AddScoped<AccountLinkService>();
			services.AddScoped<RefundService>();
            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddScoped<ICertificateRepository, CertificateRepository>();
			services.AddScoped<IQuizRepository, QuizRepository>();
			services.AddScoped<IQuestionRepository, QuestionRepository>();
			services.AddScoped<IAnswerRepository, AnswerRepository>();
			services.AddScoped<IAssignmentRepository, AssignmentRepository>();
			services.AddScoped<ICartItemRepository, CartItemRepository>();
			services.AddScoped<ILearnerRepository, LearnerRepository>();
			services.AddScoped<IUserQuestRepository, UserQuestRepository>();
			services.AddScoped<IQuartzService, QuartzService>();
			services.AddScoped<ILevelRepository, LevelRepository>();
			services.AddScoped<IBoosterRepository, BoosterRepository>();
			services.AddScoped<IStudyTimeRepository, StudyTimeRepository>();
			services.AddScoped<IReportRepository, ReportRepository>();

            services.AddSingleton(provider =>
			{
				// Define the file path to the Firebase credentials
				string filePath = Path.Combine(AppContext.BaseDirectory, "Resource", "edu-quest-2003-firebase-adminsdk-gtcp6-55271f67ec.json");

				// Initialize the FirestoreDb instance
				GoogleCredential credential = GoogleCredential.FromFile(filePath);
				return FirestoreDb.Create("edu-quest-2003", new FirestoreClientBuilder
				{
					Credential = credential
				}.Build());
			});

            #endregion

            #region Quartz
            services.AddQuartz( q =>
			{
                string cronExpression = "0 0 5 * * ?";
                var resetDailyQuests = new JobKey("resetDailyQuests");
                var resetQuestsProgress = new JobKey("resetQuestsProgress");
                q.AddJob<ResetQuestProgress>(opts => opts.WithIdentity(resetQuestsProgress));
                q.AddJob<ResetDailyQuest>(opts => opts.WithIdentity(resetDailyQuests));
                q.AddTrigger(opts => opts.ForJob(resetDailyQuests)
					.WithCronSchedule(cronExpression)
					/*.StartNow()
					.WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever())
                    .WithDescription("Reset Daily Quests!")*/
                );
                q.AddTrigger(opts => opts.ForJob(resetQuestsProgress)
                    .WithCronSchedule(cronExpression)
                    /*.StartNow()
                    .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever())
                    .WithDescription("Reset Quests Progress!")*/
                );
            });
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
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
