﻿using System.Collections.Generic;
using ReminderXamarin.Helpers;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace ReminderXamarin.Models
{
    [Table(ConstantHelper.Users)]
    public class UserModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public byte[] ImageContent { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Note> Notes { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<AchievementModel> Achievements { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<BirthdayModel> Birthdays { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<ToDoModel> ToDoModels { get; set; }
    }
}