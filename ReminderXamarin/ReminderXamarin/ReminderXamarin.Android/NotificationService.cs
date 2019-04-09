using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using System.Threading;
using Plugin.LocalNotifications;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;

namespace ReminderXamarin.Droid
{
    [Service]
    public class NotificationService : Service
    {
        private CancellationTokenSource _cts;
        private readonly List<KeyValuePair<DateTime, string>> _toDoList = new List<KeyValuePair<DateTime, string>>();

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            MessagingCenter.Subscribe<ToDoViewModel, KeyValuePair<DateTime, string>>(this, "ToDoCreated",
                (model, pair) =>
                {
                    _toDoList.Add(pair);
                });
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                var currentDate = DateTime.Now;
                for (int i = 0; i < _toDoList.Count; i++)
                {
                    var pair = _toDoList[i];
                    var dateString = currentDate.ToString("dd.MM.yyyy HH:mm");
                    var pairDateString = pair.Key.ToString("dd.MM.yyyy HH:mm");
                    if (dateString == pairDateString)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            CrossLocalNotifications.Current.Show(pair.Key.ToString("D"), pair.Value);
                        });
                        _toDoList.Remove(pair);
                    }
                }
                return true;
            });
            return StartCommandResult.Sticky;
        }
    }
}