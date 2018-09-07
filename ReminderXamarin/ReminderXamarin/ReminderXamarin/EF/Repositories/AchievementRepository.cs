using Microsoft.EntityFrameworkCore;
using ReminderXamarin.EF.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderXamarin.EF.Repositories
{
    /// <summary>
    /// Implementation of <see cref="IRepository{TEntity}" />.
    /// </summary>
    public class AchievementRepository : IDisposable, IRepository<AchievementModel>
    {
        private readonly DbContext _dbContext;
        private bool _disposed;

        public AchievementRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected DbSet<AchievementModel> DbSet => _dbContext.Set<AchievementModel>();

        public IQueryable<AchievementModel> GetAll(int userId)
        {
            return DbSet.Where(x => x.UserId == userId).Include(x => x.AchievementNotes);
        }

        public async Task<AchievementModel> GetByIdAsync(int? id)
        {
            var achievement = await DbSet.FirstOrDefaultAsync(x => x.Id == id);
            if (achievement != null)
            {
                var achievementNotes = await _dbContext.Entry(achievement).Collection(x => x.AchievementNotes).Query().ToListAsync();
                achievement.AchievementNotes = achievementNotes;
            }
            return achievement;
        }

        public async Task CreateAsync(AchievementModel item)
        {
            await DbSet.AddAsync(item);
        }

        public async Task<AchievementModel> DeleteAsync(int? id)
        {
            var item = await DbSet.FirstOrDefaultAsync(x => x.Id == id);
            if (item != null)
            {
                DbSet.Remove(item);
            }
            return item;
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Update(AchievementModel item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
        }
    }
}
