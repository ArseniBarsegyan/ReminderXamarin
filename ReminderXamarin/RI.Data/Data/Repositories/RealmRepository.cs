using System.Linq;
using Realms;
using RI.Data.Data.Core;

namespace RI.Data.Data.Repositories
{
    public class RealmRepository<TEntity> : RealmReadonlyRepository<TEntity>, IRepository<TEntity>
        where TEntity : RealmObject, IEntity
    {
        public RealmRepository(Realm realmInstance)
            : base(realmInstance)
        {
        }

        public void Create(TEntity entity)
        {
            RealmInstance.Write(() =>
            {
                RealmInstance.Add(entity);
            });
        }

        public void Update(TEntity entity)
        {
            RealmInstance.Write(() =>
            {
                RealmInstance.Add(entity, true);
            });
        }

        public TEntity Delete(object id)
        {
            TEntity entity = null;
            RealmInstance.Write(() =>
            {
                entity = RealmInstance.All<TEntity>().FirstOrDefault(x => x.Id == (string)id);
                if (entity != null)
                {
                    Delete(entity);
                }
            });
            return entity;
        }

        public void Delete(TEntity entity)
        {
            RealmInstance.Write(() =>
            {
                RealmInstance.Remove(entity);
            });
        }

        public void Save()
        {
        }
    }
}