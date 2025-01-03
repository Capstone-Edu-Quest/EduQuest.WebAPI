using EduQuest_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduQuest_Infrastructure.Persistence.EntityTypeConfigurations
{
	public class EntityTypeConfiguration :
		IEntityTypeConfiguration<Role>, IEntityTypeConfiguration<User>, IEntityTypeConfiguration<AccountPackage>,
		IEntityTypeConfiguration<Achievement>, IEntityTypeConfiguration<Answer>, IEntityTypeConfiguration<Badge>,
		IEntityTypeConfiguration<Cart>, IEntityTypeConfiguration<Certificate>, IEntityTypeConfiguration<Course>,
		IEntityTypeConfiguration<CourseAchievement>, IEntityTypeConfiguration<FavoriteList>, IEntityTypeConfiguration<Feedback>,
		IEntityTypeConfiguration<Item>, IEntityTypeConfiguration<Leaderboard>, IEntityTypeConfiguration<LearnerStatistic>,
		IEntityTypeConfiguration<LearningHistory>, IEntityTypeConfiguration<LearningMaterial>, IEntityTypeConfiguration<LearningPath>,
		IEntityTypeConfiguration<LearningPathCourse>, IEntityTypeConfiguration<PackagePrivilege>, IEntityTypeConfiguration<Payment>,
		IEntityTypeConfiguration<Question>, IEntityTypeConfiguration<Quiz>, IEntityTypeConfiguration<QuizAttempt>,
		IEntityTypeConfiguration<Reward>, IEntityTypeConfiguration<SearchHistory>, IEntityTypeConfiguration<Setting>,
		IEntityTypeConfiguration<Stage>, IEntityTypeConfiguration<Tag>, IEntityTypeConfiguration<Transaction>,
		IEntityTypeConfiguration<UserStatistic>
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

		#region Badge
		public void Configure(EntityTypeBuilder<Badge> builder)
		{

		}
		#endregion

		#region Cart
		public void Configure(EntityTypeBuilder<Cart> builder)
		{
			builder.HasOne(d => d.User)
				.WithMany(p => p.Carts)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull);

			builder.HasOne(d => d.Course)
				.WithMany(p => p.Carts)
				.HasForeignKey(d => d.CourseId)
				.OnDelete(DeleteBehavior.ClientSetNull);

			builder.HasMany(c => c.Payments)
				.WithOne(p => p.Cart)
				.HasForeignKey(p => p.CartId)
				.OnDelete(DeleteBehavior.Cascade);

		}
		#endregion

		#region Certificate
		public void Configure(EntityTypeBuilder<Certificate> builder)
		{

			builder.HasOne(d => d.User)
				.WithMany(p => p.Certificates)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.ClientSetNull);

			builder.HasOne(d => d.Course)
				.WithMany(p => p.Certificates)
				.HasForeignKey(d => d.CourseId)
				.OnDelete(DeleteBehavior.ClientSetNull);
		}
		#endregion

		#region Course
		public void Configure(EntityTypeBuilder<Course> builder)
		{

			
		}
		#endregion

		#region CourseAchievement
		public void Configure(EntityTypeBuilder<CourseAchievement> builder)
		{


		}
		#endregion

		#region FavoriteList
		public void Configure(EntityTypeBuilder<FavoriteList> builder)
		{


		}
		#endregion

		#region Feedback
		public void Configure(EntityTypeBuilder<Feedback> builder)
		{


		}
		#endregion

		#region Item
		public void Configure(EntityTypeBuilder<Item> builder)
		{


		}
		#endregion

		#region Leaderboard
		public void Configure(EntityTypeBuilder<Leaderboard> builder)
		{


		}
		#endregion

		#region LearnerStatistic
		public void Configure(EntityTypeBuilder<LearnerStatistic> builder)
		{


		}
		#endregion

		#region LearningHistory
		public void Configure(EntityTypeBuilder<LearningHistory> builder)
		{


		}
		#endregion

		#region LearningMaterial
		public void Configure(EntityTypeBuilder<LearningMaterial> builder)
		{


		}
		#endregion

		#region LearningPath
		public void Configure(EntityTypeBuilder<LearningPath> builder)
		{


		}
		#endregion

		#region LearningPathCourse
		public void Configure(EntityTypeBuilder<LearningPathCourse> builder)
		{


		}
		#endregion

		#region PackagePrivilege
		public void Configure(EntityTypeBuilder<PackagePrivilege> builder)
		{


		}
		#endregion

		#region Payment
		public void Configure(EntityTypeBuilder<Payment> builder)
		{


		}
		#endregion

		#region Question
		public void Configure(EntityTypeBuilder<Question> builder)
		{


		}
		#endregion

		#region Quiz
		public void Configure(EntityTypeBuilder<Quiz> builder)
		{


		}
		#endregion

		#region QuizAttempt
		public void Configure(EntityTypeBuilder<QuizAttempt> builder)
		{


		}
		#endregion

		#region Reward
		public void Configure(EntityTypeBuilder<Reward> builder)
		{


		}
		#endregion

		#region SearchHistory
		public void Configure(EntityTypeBuilder<SearchHistory> builder)
		{


		}
		#endregion

		#region Setting
		public void Configure(EntityTypeBuilder<Setting> builder)
		{


		}
		#endregion

		#region Stage
		public void Configure(EntityTypeBuilder<Stage> builder)
		{


		}
		#endregion

		#region Tag
		public void Configure(EntityTypeBuilder<Tag> builder)
		{


		}
		#endregion

		#region Transaction
		public void Configure(EntityTypeBuilder<Transaction> builder)
		{


		}
		#endregion

		#region UserStatistic
		public void Configure(EntityTypeBuilder<UserStatistic> builder)
		{


		}
		#endregion
	}
}
