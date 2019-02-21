﻿using System.Collections.Generic;
using Rm.Data.Core;

namespace Rm.Data.Entities
{
    public class AppUser : IEntity<string>
    {
        public AppUser()
        {
            Notes = new List<Note>();
            Achievements = new List<AchievementModel>();
            Birthdays = new List<BirthdayModel>();
            ToDoModels = new List<ToDoModel>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public byte[] Password { get; set; }
        public byte[] ImageContent { get; set; }

        public List<Note> Notes { get; set; }
        public List<AchievementModel> Achievements { get; set; }
        public List<BirthdayModel> Birthdays { get; set; }
        public List<ToDoModel> ToDoModels { get; set; }
    }
}