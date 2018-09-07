using Microsoft.EntityFrameworkCore;
using ReminderXamarin.EF.Models;

namespace ReminderXamarin.EF
{
    public class ApplicationContext : DbContext
    {
        private readonly string _databasePath;       

        public ApplicationContext(string databasePath)
        {
            _databasePath = databasePath;
            Database.EnsureCreated();
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<AchievementModel> Achievements { get; set; }
        public DbSet<AchievementNoteModel> AchievementNotes { get; set; }
        public DbSet<BirthdayModel> Birthdays { get; set; }
        public DbSet<NoteModel> Notes { get; set; }
        public DbSet<PhotoModel> Photos { get; set; }
        public DbSet<ToDoModel> ToDoModels { get; set; }
        public DbSet<VideoModel> Videos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_databasePath}");            
        }
    }
}
