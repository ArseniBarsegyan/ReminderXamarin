using System.Collections.Generic;
using Rm.Data.Data.Entities;
using SQLite;
using SQLiteNetExtensions.Extensions;

namespace Rm.Data.Data.Repositories
{
    public class AchievementStepRepository
    {
        private readonly SQLiteConnection _db;

        public AchievementStepRepository(string dbPath)
        {
            _db = new SQLiteConnection(dbPath);
            _db.CreateTable<AchievementStep>();
        }

        /// <summary>
        /// Get all achievement steps from database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AchievementStep> GetAll()
        {
            return _db.GetAllWithChildren<AchievementStep>();
        }

        /// <summary>
        /// Get achievement step from database by id.
        /// </summary>
        /// <param name="id">Id of the achievement</param>
        /// <returns></returns>
        public AchievementStep GetAchievementStepAsync(int id)
        {
            return _db.GetWithChildren<AchievementStep>(id);
        }

        /// <summary>
        /// Create (if id = 0) or update achievement step in database.
        /// </summary>
        /// <param name="achievementStep">achievement step to be saved.</param>
        /// <returns></returns>
        public void Save(AchievementStep achievementStep)
        {
            if (achievementStep.Id != 0)
            {
                _db.InsertOrReplaceWithChildren(achievementStep);
            }
            else
            {
                _db.InsertWithChildren(achievementStep);
            }
        }

        /// <summary>
        /// Delete achievement step from database.
        /// </summary>
        /// <param name="achievementStep">achievement step to be deleted</param>
        /// <returns></returns>
        public int DeleteAchievement(AchievementStep achievementStep)
        {
            return _db.Delete(achievementStep);
        }
    }
}