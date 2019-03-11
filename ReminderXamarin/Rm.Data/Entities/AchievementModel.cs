﻿using System.Collections.Generic;
using Rm.Data.Core;

namespace Rm.Data.Entities
{
    public class AchievementModel : Entity
    {
        public AchievementModel()
        {
            AchievementNotes = new List<AchievementNote>();
        }

        public byte[] ImageContent { get; set; }
        public string Title { get; set; }
        public string GeneralDescription { get; set; }
        public int GeneralTimeSpent { get; set; }

        public string UserId { get; set; }
        public List<AchievementNote> AchievementNotes { get; set; }
    }
}