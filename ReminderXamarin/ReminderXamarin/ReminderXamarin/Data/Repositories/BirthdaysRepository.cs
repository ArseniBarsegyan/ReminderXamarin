using System.Collections.Generic;
using System.Linq;
using ReminderXamarin.Data.Entities;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace ReminderXamarin.Data.Repositories
{
    public class BirthdaysRepository
    {
        private readonly SQLiteConnection _db;
        private readonly List<BirthdayModel> _birthdays;

        public BirthdaysRepository(string dbPath)
        {
            _db = new SQLiteConnection(dbPath);
            _db.CreateTable<BirthdayModel>();
            _birthdays = new List<BirthdayModel>(_db.GetAllWithChildren<BirthdayModel>());
        }

        /// <summary>
        /// Get all birthdays from database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BirthdayModel> GetAll()
        {
            return _birthdays;
        }

        /// <summary>
        /// Get birthday from database by id.
        /// </summary>
        /// <param name="id">Id of the birthday</param>
        /// <returns></returns>
        public BirthdayModel GetBirthdayAsync(int id)
        {
            return _birthdays.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Create (if id = 0) or update BirthdayModel in database.
        /// </summary>
        /// <param name="model">BirthdayModel to be saved</param>
        /// <returns></returns>
        public void Save(BirthdayModel model)
        {
            if (model.Id != 0)
            {
                _birthdays.Insert(model.Id, model);
                _db.InsertOrReplaceWithChildren(model);
            }
            else
            {
                _birthdays.Add(model);
                _db.InsertWithChildren(model);
            }
        }

        /// <summary>
        /// Delete Birthday model from database.
        /// </summary>
        /// <param name="model">BirthdayModel to be deleted</param>
        /// <returns></returns>
        public int DeleteBirthday(BirthdayModel model)
        {
            _birthdays.Remove(model);
            return _db.Delete(model);
        }
    }
}