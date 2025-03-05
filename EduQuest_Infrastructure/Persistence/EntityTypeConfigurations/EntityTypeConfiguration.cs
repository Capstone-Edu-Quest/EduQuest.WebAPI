using EduQuest_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduQuest_Infrastructure.Persistence.EntityTypeConfigurations
{
	public class EntityTypeConfiguration :
		IEntityTypeConfiguration<Role>, IEntityTypeConfiguration<User>, IEntityTypeConfiguration<AccountPackage>,
		IEntityTypeConfiguration<Answer>, IEntityTypeConfiguration<Assignment>, IEntityTypeConfiguration<Badge>,
		IEntityTypeConfiguration<Cart>, IEntityTypeConfiguration<CartItem>, IEntityTypeConfiguration<Certificate>, IEntityTypeConfiguration<Course>, IEntityTypeConfiguration<CourseStatistic>,
		IEntityTypeConfiguration<FavoriteList>, IEntityTypeConfiguration<Feedback>,
		IEntityTypeConfiguration<Item>, IEntityTypeConfiguration<Leaderboard>, IEntityTypeConfiguration<CourseLearner>,
		IEntityTypeConfiguration<LearningHistory>, IEntityTypeConfiguration<LearningMaterial>, IEntityTypeConfiguration<LearningPath>,
		IEntityTypeConfiguration<LearningPathCourse>, IEntityTypeConfiguration<Level>, IEntityTypeConfiguration<PackagePrivilege>, IEntityTypeConfiguration<Payment>, IEntityTypeConfiguration<Quest>,
		IEntityTypeConfiguration<Question>, IEntityTypeConfiguration<Quiz>, IEntityTypeConfiguration<QuizAttempt>,
		 IEntityTypeConfiguration<SearchHistory>, IEntityTypeConfiguration<Setting>,
		IEntityTypeConfiguration<Stage>, IEntityTypeConfiguration<Tag>, IEntityTypeConfiguration<Transaction>,
		IEntityTypeConfiguration<UserStatistic>, IEntityTypeConfiguration<RefreshToken>,
		IEntityTypeConfiguration<SystemConfig>, IEntityTypeConfiguration<MascotInventory>, IEntityTypeConfiguration<Coupon>, IEntityTypeConfiguration<UserCoupon>, IEntityTypeConfiguration<UserQuest>,
		IEntityTypeConfiguration<QuestReward>, IEntityTypeConfiguration<Report>

    {
		#region Role
		public void Configure(EntityTypeBuilder<Role> builder)
		{

		}
        #endregion

        #region User
        public void Configure(EntityTypeBuilder<User> builder)
        {
            
            builder.Property(u => u.Username)
                .IsRequired(false);  

            builder.Property(u => u.AvatarUrl)
                .IsRequired(false); 

            builder.Property(u => u.Email)
                .IsRequired(false); 

            builder.Property(u => u.Phone)
                .IsRequired(false); 

            builder.Property(u => u.Headline)
                .IsRequired(false); 

            builder.Property(u => u.Description)
                .IsRequired(false);  

            builder.Property(u => u.RoleId)
                .IsRequired(false);  

            builder.Property(u => u.PackagePrivilegeId)
                .IsRequired(false);  

            builder.Property(u => u.AccountPackageId)
                .IsRequired(false); 

            builder.HasOne(d => d.Role)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(u => u.MascotItem)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);

            builder.HasMany(u => u.RefreshTokens)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);

            builder.HasMany(u => u.Courses)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasMany(u => u.Carts)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.FavoriteLists)
                .WithOne(fl => fl.User)
                .HasForeignKey(fl => fl.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        #endregion

        #region AccountPackage
        public void Configure(EntityTypeBuilder<AccountPackage> builder)
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

		#region Assignment
		public void Configure(EntityTypeBuilder<Assignment> builder)
		{


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

			

			builder.HasMany(c => c.Payments)
				.WithOne(p => p.Cart)
				.HasForeignKey(p => p.CartId)
				.OnDelete(DeleteBehavior.Cascade);

		}
		#endregion

		#region CartItem
		public void Configure(EntityTypeBuilder<CartItem> builder)
		{
			

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
			builder.HasOne(c => c.User)
				.WithMany(u => u.Courses)
				.HasForeignKey(c => c.CreatedBy)
				.OnDelete(DeleteBehavior.ClientSetNull);

			

			builder.HasMany(c => c.FavoriteLists)
				.WithOne(fl => fl.Course)
				.HasForeignKey(fl => fl.CourseId)
				.OnDelete(DeleteBehavior.Cascade);

		}
        #endregion

        #region Coupon
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.HasOne(c => c.User)
                .WithMany(u => u.Coupons)
                .HasForeignKey(c => c.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);
        }
        #endregion

        #region UserCoupon
        public void Configure(EntityTypeBuilder<UserCoupon> builder)
        {
            builder.HasKey(fl => new { fl.UserId, fl.CouponId });

        }
        #endregion

        #region CourseStatistic
        public void Configure(EntityTypeBuilder<CourseStatistic> builder)
		{
			

		}
		#endregion

		#region FavoriteList
		public void Configure(EntityTypeBuilder<FavoriteList> builder)
		{
			

			// Define the relationship with User
			builder.HasOne(fl => fl.User)
				.WithMany(u => u.FavoriteLists)
				.HasForeignKey(fl => fl.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			// Define the relationship with Course
			builder.HasOne(fl => fl.Course)
				.WithMany(c => c.FavoriteLists)
				.HasForeignKey(fl => fl.CourseId)
				.OnDelete(DeleteBehavior.Cascade);

		}
		#endregion

		#region Feedback
		public void Configure(EntityTypeBuilder<Feedback> builder)
		{
			/*builder.HasKey(fl => new { fl.UserId, fl.CourseId });*/

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

		#region Learner
		public void Configure(EntityTypeBuilder<CourseLearner> builder)
		{


		}
        #endregion

        #region Report
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.HasOne(r => r.User)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.Reporter)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(r => r.Feedback)
                .WithMany(f => f.Reports)
                .HasForeignKey(r => r.FeedbackId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(r => r.Course)
                .WithMany(c => c.Reports)
                .HasForeignKey(r => r.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne<User>()
					.WithMany()
					.HasForeignKey(r => r.Violator)
					.OnDelete(DeleteBehavior.ClientSetNull);

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
			builder.HasKey(fl => new { fl.LearningPathId, fl.CourseId });

		}
        #endregion

        #region Level
        public void Configure(EntityTypeBuilder<Level> builder)
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

		#region Quest
		public void Configure(EntityTypeBuilder<Quest> builder)
		{
            builder.HasOne(q => q.User)
                .WithMany(c => c.Quests)
                .HasForeignKey(q => q.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
        #endregion

        #region UserQuest
        public void Configure(EntityTypeBuilder<UserQuest> builder)
        {
            builder.HasOne(q => q.User)
                .WithMany(c => c.UserQuests)
                .HasForeignKey(q => q.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
        #endregion
        #region QuestReward
        public void Configure(EntityTypeBuilder<QuestReward> builder)
        {
            builder.HasOne(qr => qr.Quest)
                .WithMany(q => q.Rewards)
                .HasForeignKey(qr => qr.QuestId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(qr => qr.UserQuest)
                .WithMany(uq => uq.Rewards)
                .HasForeignKey(qr => qr.UserQuestId)
                .OnDelete(DeleteBehavior.ClientSetNull);
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

		#region SystemConfig
		public void Configure(EntityTypeBuilder<SystemConfig> builder)
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

		#region UserMascot
		public void Configure(EntityTypeBuilder<MascotInventory> builder)
		{


		}
		#endregion
	
        #region Refresh token
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            
        }
        #endregion


    }
}
