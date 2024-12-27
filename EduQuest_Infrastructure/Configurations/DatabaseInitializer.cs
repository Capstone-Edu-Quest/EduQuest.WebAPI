using EduQuest_Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduQuest_Infrastructure.Configurations
{
	public static class DatabaseInitializer
	{
		private static readonly Random _rand = new();

		public static async Task InitializeAsync(ApplicationDbContext dbContext)
		{
			dbContext.Database.EnsureCreated();
		}
	}
}
