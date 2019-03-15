using System;
using Realms;
using RI.Data.Data.Core;

namespace RI.Data.Data.Entities
{
    public class AchievementNote : RealmObject, IEntity
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Description { get; set; }
        public int HoursSpent { get; set; }
        public DateTimeOffset Date { get; set; }

        public AchievementModel Achievement { get; set; }
    }
}