using EduQuest_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduQuest_Infrastructure.Persistence.EntityTypeConfigurations
{
	public class EntityTypeConfiguration :
		IEntityTypeConfiguration<Role>, IEntityTypeConfiguration<User>, IEntityTypeConfiguration<AccountPackage>,
		IEntityTypeConfiguration<Achievement>, IEntityTypeConfiguration<Answer>
	{
		#region Role
		public void Configure(EntityTypeBuilder<Role> builder)
		{

		}
		#endregion

		#region User
		public void Configure(EntityTypeBuilder<User> builder)
		{

			builder.HasOne(d => d.Role)
				.WithMany(p => p.Users)
				.HasForeignKey(d => d.RoleId)
				.OnDelete(DeleteBehavior.ClientSetNull);
		}

		#endregion

		#region AccountPackage
		public void Configure(EntityTypeBuilder<AccountPackage> builder)
		{

			
		}

		#endregion

		#region Achievement
		public void Configure(EntityTypeBuilder<Achievement> builder)
		{

		}
		#endregion

		#region Answer
		public void Configure(EntityTypeBuilder<Answer> builder)
		{

			builder.HasOne(d => d.Question)
				.WithMany(p => p.Answers)
				.HasForeignKey(d => d.QuestionId)
				.OnDelete(DeleteBehavior.ClientSetNull);
		}
		#endregion
	}
}
