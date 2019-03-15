using System;
using System.Collections.Generic;
using Realms;
using RI.Data.Data.Core;

namespace RI.Data.Data.Entities
{
    public class AppUser : RealmObject, IEntity
    {
        public AppUser()
        {
            Notes = new List<Note>();
            Achievements = new List<AchievementModel>();
            Birthdays = new List<BirthdayModel>();
            ToDoModels = new List<ToDoModel>();
        }

        [PrimaryKey]
        public string Id { get; set; }
        public string UserName { get; set; }
        public byte[] Password { get; set; }
        public byte[] ImageContent { get; set; }

        public IList<Note> Notes { get; }
        public IList<AchievementModel> Achievements { get; }
        public IList<BirthdayModel> Birthdays { get; }
        public IList<ToDoModel> ToDoModels { get; }
    }
}