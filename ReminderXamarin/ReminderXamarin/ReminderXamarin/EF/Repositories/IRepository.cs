using ReminderXamarin.EF.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderXamarin.EF
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        IQueryable<TEntity> GetAll(int userId);
        Task CreateAsync(TEntity item);
        Task<TEntity> GetByIdAsync(int? id);
        void Update(TEntity item);
        Task<TEntity> DeleteAsync(int? id);
        Task SaveAsync();
    }
}
