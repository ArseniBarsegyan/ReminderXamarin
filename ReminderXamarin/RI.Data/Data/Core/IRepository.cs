using System.Threading.Tasks;
using Realms;

namespace RI.Data.Data.Core
{
    /// <summary>
    /// Provide CRUD methods for datasource.
    /// </summary>
    /// <typeparam name="TEntity">Entity type that inherit <see cref="IEntity{T}"/></typeparam>
    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity>
        where TEntity : RealmObject, IEntity
    {
        void Create(TEntity entity);
        void Update(TEntity entity);
        TEntity Delete(object id);
        void Delete(TEntity entity);
        void Save();
    }
}