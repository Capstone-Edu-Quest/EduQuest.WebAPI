using EduQuest_Domain.Entities;
using EduQuest_Domain.Repository.UnitOfWork;
using EduQuest_Infrastructure.Extensions;
using EduQuest_Infrastructure.Persistence.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace EduQuest_Infrastructure.Persistence
{
	public class ApplicationDbContext : DbContext, IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

		public virtual DbSet<User> Users { get; set; } = null!;
		public virtual DbSet<Role> Roles { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(EntityTypeConfiguration).Assembly);
			foreach (var entityType in modelBuilder.Model.GetEntityTypes())
			{
				if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
				{
					entityType.AddSoftDeleteQueryFilter();
				}
			}
		}
    }

}
