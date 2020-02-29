using Rm.Data.Data.Entities;

using SQLite;

using SQLiteNetExtensions.Extensions;

using System.Collections.Generic;

namespace Rm.Data.Data.Repositories
{
    public class ToDoRepository
    {
        private readonly SQLiteConnection _db;

        public ToDoRepository(string dbPath)
        {
            _db = new SQLiteConnection(dbPath);
            _db.CreateTable<ToDoModel>();
        }

        public IEnumerable<ToDoModel> GetAll()
        {
            return _db.GetAllWithChildren<ToDoModel>();
        }

        public ToDoModel GetToDoAsync(int id)
        {
            return _db.GetWithChildren<ToDoModel>(id);
        }

        public void Save(ToDoModel model)
        {
            if (model.Id != 0)
            {
                _db.InsertOrReplaceWithChildren(model);
            }
            else
            {
                _db.InsertWithChildren(model);
            }
        }

        public int DeleteModel(ToDoModel model)
        {
            return _db.Delete(model);
        }
    }
}