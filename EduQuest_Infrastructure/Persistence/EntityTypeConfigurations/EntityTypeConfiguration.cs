using EduQuest_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduQuest_Infrastructure.Persistence.EntityTypeConfigurations
{
	public class EntityTypeConfiguration :
		IEntityTypeConfiguration<Role>, IEntityTypeConfiguration<User>
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
	}
}
