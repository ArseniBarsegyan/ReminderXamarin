using Rm.Data.Data.Entities;

using SQLite;

using SQLiteNetExtensions.Extensions;

using System.Collections.Generic;

namespace Rm.Data.Data.Repositories
{
    public class AchievementRepository
    {
        private readonly SQLiteConnection _db;

        public AchievementRepository(string dbPath)
        {
            _db = new SQLiteConnection(dbPath);
            _db.CreateTable<AchievementModel>();
            _db.CreateTable<AchievementStep>();
        }

        public IEnumerable<AchievementModel> GetAll()
        {
            return _db.GetAllWithChildren<AchievementModel>();
        }

        public AchievementModel GetAchievementAsync(int id)
        {
            return _db.GetWithChildren<AchievementModel>(id);
        }

        public void Save(AchievementModel achievement)
        {
            if (achievement.Id != 0)
            {
                _db.InsertOrReplaceWithChildren(achievement);
            }
            else
            {
                _db.InsertWithChildren(achievement);
            }
        }

        public int DeleteAchievement(AchievementModel achievement)
        {
            return _db.Delete(achievement);
        }
    }
}