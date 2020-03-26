using Rm.Data.Data.Entities;

using SQLite;

using SQLiteNetExtensions.Extensions;

using System.Collections.Generic;

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

        public IEnumerable<AchievementStep> GetAll()
        {
            return _db.GetAllWithChildren<AchievementStep>();
        }

        public AchievementStep GetAchievementStepAsync(int id)
        {
            return _db.GetWithChildren<AchievementStep>(id);
        }

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

        public int DeleteAchievementStep(AchievementStep achievementStep)
        {
            return _db.Delete(achievementStep);
        }
    }
}