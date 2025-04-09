using EduQuest_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EduQuest_Infrastructure.Persistence.EntityTypeConfigurations
{
	public class EntityTypeConfiguration :
		IEntityTypeConfiguration<Role>, IEntityTypeConfiguration<User>, IEntityTypeConfiguration<Subscription>,
		IEntityTypeConfiguration<Answer>, IEntityTypeConfiguration<Assignment>, IEntityTypeConfiguration<Advertise>,
		IEntityTypeConfiguration<Cart>, IEntityTypeConfiguration<CartItem>, IEntityTypeConfiguration<Certificate>, IEntityTypeConfiguration<Course>, IEntityTypeConfiguration<CourseStatistic>,
		IEntityTypeConfiguration<FavoriteList>, IEntityTypeConfiguration<Feedback>,
		IEntityTypeConfiguration<Item>, IEntityTypeConfiguration<CourseLearner>,
		IEntityTypeConfiguration<LearningHistory>, IEntityTypeConfiguration<Material>, IEntityTypeConfiguration<LearningPath>,
		IEntityTypeConfiguration<Question>, IEntityTypeConfiguration<Quiz>, IEntityTypeConfiguration<QuizAttempt>,
		 IEntityTypeConfiguration<SearchHistory>, IEntityTypeConfiguration<Setting>,
		IEntityTypeConfiguration<Lesson>, IEntityTypeConfiguration<Tag>, IEntityTypeConfiguration<Transaction>, IEntityTypeConfiguration<TransactionDetail>,
		IEntityTypeConfiguration<UserMeta>, IEntityTypeConfiguration<RefreshToken>,
		IEntityTypeConfiguration<SystemConfig>, IEntityTypeConfiguration<Mascot>, IEntityTypeConfiguration<Coupon>, IEntityTypeConfiguration<UserCoupon>, IEntityTypeConfiguration<UserQuest>,
		IEntityTypeConfiguration<Report>, IEntityTypeConfiguration<Booster>, IEntityTypeConfiguration<AssignmentAttempt>, IEntityTypeConfiguration<AssignmentPeerReview>,
		IEntityTypeConfiguration<UserQuizAnswers>

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

            builder.HasMany(u => u.FavoriteLists)
                .WithOne(fl => fl.User)
                .HasForeignKey(fl => fl.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        #endregion

        #region AccountPackage
        public void Configure(EntityTypeBuilder<Subscription> builder)
		{

			
		}

        #endregion

        #region Booster
        public void Configure(EntityTypeBuilder<Booster> builder)
        {
			builder.HasOne(c => c.User)
				.WithMany(u => u.Boosters)
				.HasForeignKey(c => c.UserId)
				.OnDelete(DeleteBehavior.Cascade);
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

		#region Advertise
		public void Configure(EntityTypeBuilder<Advertise> builder)
		{

			
		}
		#endregion

		#region Assignment
		public void Configure(EntityTypeBuilder<Assignment> builder)
		{


		}
        public void Configure(EntityTypeBuilder<AssignmentAttempt> builder)
        {


        }
        public void Configure(EntityTypeBuilder<AssignmentPeerReview> builder)
        {
			builder.HasOne<User>()
				.WithMany()
				.HasForeignKey(r => r.ReviewerId)
				.OnDelete(DeleteBehavior.Cascade);
        }
        #endregion

        #region Cart
        public void Configure(EntityTypeBuilder<Cart> builder)
		{

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

			

			

		}
        #endregion

        #region Coupon
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.HasOne(c => c.User)
                .WithMany(u => u.Coupons)
                .HasForeignKey(c => c.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);

            /*builder.HasMany(c => c.WhiteListUsers)
			.WithMany()
			.UsingEntity<Dictionary<string, object>>(
            "CouponUser", // Tên của bảng trung gian
            j => j
                .HasOne<User>()
                .WithMany()
                .HasForeignKey("UserId") // Tên của khóa ngoại trong bảng trung gian
                .OnDelete(DeleteBehavior.Cascade),
            j => j
                .HasOne<Coupon>()
                .WithMany()
                .HasForeignKey("CouponId"), // Tên của khóa ngoại trong bảng trung gian
            j =>
            {
                j.HasKey("CouponId", "UserId"); // Khóa chính của bảng trung gian
            });*/
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

		#region Material
		public void Configure(EntityTypeBuilder<Material> builder)
		{
            builder
			.HasOne(a => a.Assignment)
			.WithOne(m => m.Material)
			.HasForeignKey<Material>(m => m.AssignmentId);

            builder
            .HasOne(a => a.Quiz)
            .WithOne(m => m.Material)
            .HasForeignKey<Material>(m => m.QuizId);

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
        public void Configure(EntityTypeBuilder<Levels> builder)
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
            builder.HasOne<Quest>()
                    .WithMany()
                    .HasForeignKey(r => r.QuestId)
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
        public void Configure(EntityTypeBuilder<UserQuizAnswers> builder)
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
		public void Configure(EntityTypeBuilder<Lesson> builder)
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

		#region TransactionDetail
		public void Configure(EntityTypeBuilder<TransactionDetail> builder)
		{


		}
		#endregion

		#region UserStatistic
		public void Configure(EntityTypeBuilder<UserMeta> builder)
		{


		}
		#endregion

		#region UserMascot
		public void Configure(EntityTypeBuilder<Mascot> builder)
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
