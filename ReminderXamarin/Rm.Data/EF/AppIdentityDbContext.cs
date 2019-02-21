using Microsoft.EntityFrameworkCore;
using Rm.Data.Entities;

namespace Rm.Data.EF
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class AppIdentityDbContext : DbContext
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="AppIdentityDbContext"/> class.
        /// </summary>
        /// <param name="dbContextOptions">The database context options.</param>
        /// <param name="schemaName">Name of the schema.</param>
        public AppIdentityDbContext(DbContextOptions dbContextOptions, string schemaName)
            : base(dbContextOptions)
        {
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
            modelBuilder.ApplyConfiguration(new AppIdentityTypeConfiguration<AppUser>("Users", SchemaName));
        }
    }
}