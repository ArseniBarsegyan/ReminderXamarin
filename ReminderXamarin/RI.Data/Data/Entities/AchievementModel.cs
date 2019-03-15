using System;
using System.Collections.Generic;
using Realms;
using RI.Data.Data.Core;

namespace RI.Data.Data.Entities
{
    public class AchievementModel : RealmObject, IEntity
    {
        public AchievementModel()
        {
            AchievementNotes = new List<AchievementNote>();
        }

        [PrimaryKey]
        public string Id { get; set; }
        public byte[] ImageContent { get; set; }
        public string Title { get; set; }
        public string GeneralDescription { get; set; }
        public int GeneralTimeSpent { get; set; }

        public string UserId { get; set; }
        public IList<AchievementNote> AchievementNotes { get; }
    }
}