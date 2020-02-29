using Rm.Helpers;

using SQLite;

using SQLiteNetExtensions.Attributes;

namespace Rm.Data.Data.Entities
{
    [Table(ConstantsHelper.AchievementSteps)]
    public class AchievementStep
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public byte[] ImageContent { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TimeSpent { get; set; }
        public int TimeEstimation { get; set; }

        [ForeignKey(typeof(AchievementModel))]
        public int AchievementId { get; set; }
    }
}