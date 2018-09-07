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
    public class ToDoRepository : IDisposable, IRepository<ToDoModel>
    {
        private readonly DbContext _dbContext;
        private bool _disposed;

        public ToDoRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected DbSet<ToDoModel> DbSet => _dbContext.Set<ToDoModel>();

        public IQueryable<ToDoModel> GetAll(int userId)
        {
            return DbSet.Where(x => x.UserId == userId);
        }

        public async Task<ToDoModel> GetByIdAsync(int? id)
        {
            var todoModel = await DbSet.FirstOrDefaultAsync(x => x.Id == id);            
            return todoModel;
        }

        public async Task CreateAsync(ToDoModel item)
        {
            await DbSet.AddAsync(item);
        }

        public async Task<ToDoModel> DeleteAsync(int? id)
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

        public void Update(ToDoModel item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
        }
    }
}
