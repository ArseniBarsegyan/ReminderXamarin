using Rm.Data.Data.Entities;

using SQLite;

using SQLiteNetExtensions.Extensions;

using System;
using System.Collections.Generic;

namespace Rm.Data.Data.Repositories
{
    public class UserRepository
    {
        private readonly SQLiteConnection _db;

        public UserRepository(string dbPath)
        {
            _db = new SQLiteConnection(dbPath);
            _db.CreateTable<UserModel>();
            _db.CreateTable<AchievementModel>();
            _db.CreateTable<BirthdayModel>();
            _db.CreateTable<Note>();
            _db.CreateTable<ToDoModel>();
        }

        public IEnumerable<UserModel> GetAll()
        {
            return _db.GetAllWithChildren<UserModel>();
        }

        public UserModel GetUserAsync(string id)
        {
            return _db.GetWithChildren<UserModel>(id);
        }

        public void Save(UserModel user)
        {
            if (!string.IsNullOrEmpty(user.Id))
            {
                var userToUpdate = _db.Get<UserModel>(user.Id);
                userToUpdate.UserName = user.UserName;
                userToUpdate.ImageContent = user.ImageContent;
                _db.Update(userToUpdate);
            }
            else
            {
                user.Id = Guid.NewGuid().ToString();
                _db.Insert(user);
            }
        }

        public int DeleteUser(UserModel user)
        {
            return _db.Delete(user);
        }
    }
}