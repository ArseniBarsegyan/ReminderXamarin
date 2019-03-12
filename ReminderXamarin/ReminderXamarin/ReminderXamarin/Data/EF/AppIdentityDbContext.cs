using Microsoft.EntityFrameworkCore;
using ReminderXamarin.Data.Entities;

namespace ReminderXamarin.Data.EF
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class AppIdentityDbContext : DbContext
    {
        private string _databasePath;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="AppIdentityDbContext"/> class.
        /// </summary>
        /// <param name="dbContextOptions">The database context options.</param>
        /// <param name="schemaName">Name of the schema.</param>
        /// <param name="databasePath">Path to database.</param>
        public AppIdentityDbContext(DbContextOptions dbContextOptions, string schemaName, string databasePath)
            : base(dbContextOptions)
        {
            _databasePath = databasePath;
            Database.EnsureCreated();
            SchemaName = schemaName;
        }

        /// <summary>
        /// Database schema name
        /// </summary>
        public string SchemaName { get; }

        /// <inheritdoc />
        /// <summary>
        /// Applying the configuration of dbContext entities
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new AppIdentityTypeConfiguration<AchievementModel>("Achievements", SchemaName));
            modelBuilder.ApplyConfiguration(new AppIdentityTypeConfiguration<AchievementNote>("AchievementNotes", SchemaName));
            modelBuilder.ApplyConfiguration(new AppIdentityTypeConfiguration<BirthdayModel>("Birthdays", SchemaName));
            modelBuilder.ApplyConfiguration(new AppIdentityTypeConfiguration<Note>("Notes", SchemaName));
            modelBuilder.ApplyConfiguration(new AppIdentityTypeConfiguration<PhotoModel>("Photos", SchemaName));
            modelBuilder.ApplyConfiguration(new AppIdentityTypeConfiguration<ToDoModel>("ToDoModels", SchemaName));
            modelBuilder.ApplyConfiguration(new AppIdentityTypeConfiguration<VideoModel>("Videos", SchemaName));
            modelBuilder.ApplyConfiguration(new AppIdentityTypeConfiguration<AppUser>("AppUsers", SchemaName));
        }

        /// <summary>
        /// Setup context to use SQLite database.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_databasePath}");
        }
    }
}