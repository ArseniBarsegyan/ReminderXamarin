﻿using System;
using ReminderXamarin.Helpers;
using SQLite;

namespace ReminderXamarin.Models
{
    [Table(ConstantHelper.Birthdays)]
    public class BirthdayModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public byte[] ImageContent { get; set; }
        public string Name { get; set; }
        public DateTime BirthDayDate { get; set; }
        public string GiftDescription { get; set; }
    }
}