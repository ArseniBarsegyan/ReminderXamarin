using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace ReminderXamarin.Models
{
    public class FriendsRepository
    {
        private readonly SQLiteConnection _db;

        public FriendsRepository(string dbPath)
        {
            _db = new SQLiteConnection(dbPath);
            _db.CreateTable<FriendModel>();
        }

        /// <summary>
        /// Get all friends from database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FriendModel> GetAll()
        {
            return _db.GetAllWithChildren<FriendModel>();
        }

        /// <summary>
        /// Get friend from database by id.
        /// </summary>
        /// <param name="id">Id of the note</param>
        /// <returns></returns>
        public FriendModel GetFriendAsync(int id)
        {
            return _db.GetWithChildren<FriendModel>(id);
        }

        /// <summary>
        /// Create (if id = 0) or update FriendModel in database.
        /// </summary>
        /// <param name="model">FriendModel to be saved</param>
        /// <returns></returns>
        public void Save(FriendModel model)
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

        /// <summary>
        /// Delete friend from database.
        /// </summary>
        /// <param name="model">FriendModel to be deleted</param>
        /// <returns></returns>
        public int DeleteNote(FriendModel model)
        {
            return _db.Delete(model);
        }
    }
}