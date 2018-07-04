using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace ReminderXamarin.Models
{
    public class UserRepository
    {
        private readonly SQLiteConnection _db;

        public UserRepository(string dbPath)
        {
            _db = new SQLiteConnection(dbPath);
            _db.CreateTable<UserModel>();
        }

        /// <summary>
        /// Get all users from database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserModel> GetAll()
        {
            return _db.GetAllWithChildren<UserModel>();
        }

        /// <summary>
        /// Get user from database by id.
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns></returns>
        public UserModel GetUserAsync(int id)
        {
            return _db.Get<UserModel>(id);
        }

        /// <summary>
        /// Create (if id = 0) or update note in database.
        /// </summary>
        /// <param name="user">User to be saved</param>
        /// <returns></returns>
        public void Save(UserModel user)
        {
            if (user.Id != 0)
            {
                _db.Update(user);
            }
            else
            {
                _db.Insert(user);
            }
        }

        /// <summary>
        /// Delete user from database.
        /// </summary>
        /// <param name="user">User to be deleted</param>
        /// <returns></returns>
        public int DeleteUser(UserModel user)
        {
            return _db.Delete(user);
        }
    }
}