using Rm.Data.Data.Entities;

using SQLite;

using SQLiteNetExtensions.Extensions;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rm.Data.Data.Repositories
{
    public class BirthdaysRepository
    {
        private readonly SQLiteConnection _db;

        public BirthdaysRepository(string dbPath)
        {
            _db = new SQLiteConnection(dbPath);
            _db.CreateTable<BirthdayModel>();
        }

        public IEnumerable<BirthdayModel> GetAll(Expression<Func<BirthdayModel,bool>> filter = null)
        {
            return _db.GetAllWithChildren<BirthdayModel>(filter);
        }

        public BirthdayModel GetBirthdayAsync(int id)
        {
            return _db.GetWithChildren<BirthdayModel>(id);
        }

        public void Save(BirthdayModel model)
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

        public int DeleteBirthday(BirthdayModel model)
        {
            return _db.Delete(model);
        }
    }
}