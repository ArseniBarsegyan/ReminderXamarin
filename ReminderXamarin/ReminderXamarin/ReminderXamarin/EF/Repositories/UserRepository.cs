using Microsoft.EntityFrameworkCore;
using ReminderXamarin.EF.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderXamarin.EF.Repositories
{
    public class UserRepository : IDisposable
    {
        private readonly DbContext _dbContext;
        private bool _disposed;

        public UserRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected DbSet<UserModel> DbSet => _dbContext.Set<UserModel>();

        public IQueryable<UserModel> GetAll()
        {
            return DbSet;
        }

        public async Task<UserModel> GetByIdAsync(int? id)
        {
            var user = await DbSet.FirstOrDefaultAsync(x => x.Id == id);            
            return user;
        }

        public async Task CreateAsync(UserModel item)
        {
            await DbSet.AddAsync(item);
        }

        public async Task<UserModel> DeleteAsync(int? id)
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

        public void Update(NoteModel item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
        }
    }
}
