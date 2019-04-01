using System.Collections.Generic;
using System.Linq;
using ReminderXamarin.Data.Entities;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace ReminderXamarin.Data.Repositories
{
    public class ToDoRepository
    {
        private readonly SQLiteConnection _db;
        private readonly List<ToDoModel> _toDoModels;

        public ToDoRepository(string dbPath)
        {
            _db = new SQLiteConnection(dbPath);
            _db.CreateTable<ToDoModel>();
            _toDoModels = new List<ToDoModel>(_db.GetAllWithChildren<ToDoModel>());
        }

        /// <summary>
        /// Get all models from database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ToDoModel> GetAll()
        {
            return _toDoModels;
        }

        /// <summary>
        /// Get ToDoModel from database by id.
        /// </summary>
        /// <param name="id">Id of the model</param>
        /// <returns></returns>
        public ToDoModel GetToDoAsync(int id)
        {
            return _toDoModels.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Create (if id = 0) or update model in database.
        /// </summary>
        /// <param name="model">Model to be saved</param>
        /// <returns></returns>
        public void Save(ToDoModel model)
        {
            if (model.Id != 0)
            {
                _toDoModels.Insert(model.Id, model);
                _db.InsertOrReplaceWithChildren(model);
            }
            else
            {
                _toDoModels.Add(model);
                _db.InsertWithChildren(model);
            }
        }

        /// <summary>
        /// Delete model from database.
        /// </summary>
        /// <param name="model">Model to be deleted</param>
        /// <returns></returns>
        public int DeleteModel(ToDoModel model)
        {
            _toDoModels.Remove(model);
            return _db.Delete(model);
        }
    }
}