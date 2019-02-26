using System;
using Rm.Data.Core;

namespace Rm.Data.Entities
{
    public class AchievementNote : Entity
    {
        public string Description { get; set; }
        public int HoursSpent { get; set; }
        public DateTime Date { get; set; }

        public Guid? AchievementId { get; set; }
        public AchievementModel Achievement { get; set; }
    }
}