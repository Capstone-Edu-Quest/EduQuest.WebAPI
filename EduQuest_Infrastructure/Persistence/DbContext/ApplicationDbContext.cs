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

		
		
		public virtual DbSet<Subscription> Subscriptions { get; set; } = null!;
		public virtual DbSet<Answer> Answers { get; set; } = null!;
		public virtual DbSet<Advertise> Advertises { get; set; } = null!;
		public virtual DbSet<Assignment> Assignments { get; set; } = null!;
		public virtual DbSet<Cart> Carts { get; set; } = null!;
		public virtual DbSet<CartItem> CartItems { get; set; } = null!;
		public virtual DbSet<Certificate> Certificates { get; set; } = null!;
		public virtual DbSet<Coupon> Coupon { get; set; } = null!;
		public virtual DbSet<Course> Courses { get; set; } = null!;
		public virtual DbSet<CourseStatistic> CourseStatistics { get; set; } = null!;
		public virtual DbSet<FavoriteList> FavoriteLists { get; set; } = null!;
		public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
		public virtual DbSet<Item> Items { get; set; } = null!;
		public virtual DbSet<Leaderboard> Leaderboards { get; set; } = null!;
		public virtual DbSet<CourseLearner> Learners { get; set; } = null!;
		public virtual DbSet<LearningHistory> LearningHistorys { get; set; } = null!;
		public virtual DbSet<Material> Materials { get; set; } = null!;
		public virtual DbSet<LearningPath> LearningPaths { get; set; } = null!;
		public virtual DbSet<LearningPathCourse> LearningPathCourses { get; set; } = null!;
		public virtual DbSet<Level> Levels { get; set; } = null!;
		public virtual DbSet<Mascot> Mascots { get; set; } = null!;
		public virtual DbSet<Quest> Quests { get; set; } = null!;
		public virtual DbSet<Question> Questions { get; set; } = null!;
		public virtual DbSet<Quiz> Quizzes { get; set; } = null!;
		public virtual DbSet<QuizAttempt> QuizAttempts { get; set; } = null!;
		public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
		public virtual DbSet<Role> Roles { get; set; } = null!;
		public virtual DbSet<SearchHistory> SearchHistories { get; set; } = null!;
		public virtual DbSet<Setting> Settings { get; set; } = null!;
		public virtual DbSet<ShopItem> ShopItems { get; set; } = null!;
		public virtual DbSet<Lesson> Lessons { get; set; } = null!;
		public virtual DbSet<StudyTime> StudyTimes { get; set; } = null!;
		public virtual DbSet<SystemConfig> SystemConfigs { get; set; } = null!;
		public virtual DbSet<Tag> Tags { get; set; } = null!;
		public virtual DbSet<Transaction> Transactions { get; set; } = null!;
		public virtual DbSet<TransactionDetail> TransactionDetails { get; set; } = null!;
		public virtual DbSet<User> Users { get; set; } = null!;
		public virtual DbSet<UserMeta> UserMetas { get; set; } = null!;
		public virtual DbSet<UserQuest> UserQuests { get; set; } = null!;
		public virtual DbSet<Report> Reports { get; set; } = null!;
		public virtual DbSet<LevelReward> LevelRewards { get; set; } = null!;
	

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
