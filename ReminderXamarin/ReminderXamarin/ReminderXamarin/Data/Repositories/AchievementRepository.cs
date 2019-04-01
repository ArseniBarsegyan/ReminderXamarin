using System.Collections.Generic;
using System.Linq;
using ReminderXamarin.Data.Entities;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace ReminderXamarin.Data.Repositories
{
    public class AchievementRepository
    {
        private readonly SQLiteConnection _db;
        private readonly List<AchievementModel> _achievementModels;

        public AchievementRepository(string dbPath)
        {
            _db = new SQLiteConnection(dbPath);
            _db.CreateTable<AchievementModel>();
            _db.CreateTable<AchievementNote>();
            _achievementModels = new List<AchievementModel>(_db.GetAllWithChildren<AchievementModel>());
        }

        /// <summary>
        /// Get all achievements from database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AchievementModel> GetAll()
        {
            return _achievementModels;
        }

        /// <summary>
        /// Get achievement from database by id.
        /// </summary>
        /// <param name="id">Id of the achievement</param>
        /// <returns></returns>
        public AchievementModel GetAchievementAsync(int id)
        {
            return _achievementModels.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Create (if id = 0) or update achievement in database.
        /// </summary>
        /// <param name="achievement">achievement to be saved.</param>
        /// <returns></returns>
        public void Save(AchievementModel achievement)
        {
            if (achievement.Id != 0)
            {
                _achievementModels.Insert(achievement.Id, achievement);
                _db.InsertOrReplaceWithChildren(achievement);
            }
            else
            {
                _achievementModels.Add(achievement);
                _db.Insert(achievement);
            }
        }

        /// <summary>
        /// Delete achievement from database.
        /// </summary>
        /// <param name="achievement">achievement to be deleted</param>
        /// <returns></returns>
        public int DeleteAchievement(AchievementModel achievement)
        {
            _achievementModels.Remove(achievement);
            return _db.Delete(achievement);
        }
    }
}