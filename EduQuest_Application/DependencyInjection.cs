using EduQuest_Application.Behaviour;
using EduQuest_Application.Mappings;
using EduQuest_Application.UseCases;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EduQuest_Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddMediatR(cfg =>
			{
				cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
				cfg.AddOpenBehavior(typeof(UnitOfWorkBehaviour<,>));
				cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
				cfg.RegisterServicesFromAssembly(typeof(SendMessageWithNotificationCommandHanlder).Assembly);
			});
			services.AddAutoMapper(Assembly.GetExecutingAssembly());

			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddScoped<MaxExpLevelResolver>();

            return services;
		}
	}
}
