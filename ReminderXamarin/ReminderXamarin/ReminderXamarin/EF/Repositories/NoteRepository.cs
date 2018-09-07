using Microsoft.EntityFrameworkCore;
using ReminderXamarin.EF.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderXamarin.EF
{
    /// <summary>
    /// Implementation of <see cref="IRepository{TEntity}" />.
    /// </summary>
    public class NoteRepository : IDisposable, IRepository<NoteModel>
    {
        private readonly DbContext _dbContext;
        private bool _disposed;

        public NoteRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected DbSet<NoteModel> DbSet => _dbContext.Set<NoteModel>();

        public IQueryable<NoteModel> GetAll(int userId)
        {
            return DbSet.Where(x => x.UserId == userId).Include(x => x.Photos);
        }

        public async Task<NoteModel> GetByIdAsync(int? id)
        {
            var note = await DbSet.FirstOrDefaultAsync(x => x.Id == id);
            if (note != null)
            {
                var photos = await _dbContext.Entry(note).Collection(x => x.Photos).Query().ToListAsync();
                note.Photos = photos;
            }
            return note;
        }

        public async Task CreateAsync(NoteModel item)
        {
            await DbSet.AddAsync(item);
        }

        public async Task<NoteModel> DeleteAsync(int? id)
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
