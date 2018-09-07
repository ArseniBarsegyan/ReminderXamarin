using Microsoft.EntityFrameworkCore;
using ReminderXamarin.EF.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderXamarin.EF.Repositories
{
    public class BirthdaysRepository : IDisposable, IRepository<BirthdayModel>
    {
        private readonly DbContext _dbContext;
        private bool _disposed;

        public BirthdaysRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected DbSet<BirthdayModel> DbSet => _dbContext.Set<BirthdayModel>();

        public IQueryable<BirthdayModel> GetAll(int userId)
        {
            return DbSet.Where(x => x.UserId == userId);
        }

        public async Task<BirthdayModel> GetByIdAsync(int? id)
        {
            var birthdayModel = await DbSet.FirstOrDefaultAsync(x => x.Id == id);
            return birthdayModel;
        }

        public async Task CreateAsync(BirthdayModel item)
        {
            await DbSet.AddAsync(item);
        }

        public async Task<BirthdayModel> DeleteAsync(int? id)
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

        public void Update(BirthdayModel item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
        }
    }
}
