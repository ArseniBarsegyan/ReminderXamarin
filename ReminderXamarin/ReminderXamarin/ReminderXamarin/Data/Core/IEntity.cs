﻿namespace ReminderXamarin.Data.Core
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}