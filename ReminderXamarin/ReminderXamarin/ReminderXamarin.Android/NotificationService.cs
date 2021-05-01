using Android.App;
using Android.Content;
using Android.OS;

using ReminderXamarin.DependencyResolver;
using ReminderXamarin.Services;

using System;

using Xamarin.Forms;

namespace ReminderXamarin.Droid
{
    [Service]
    public class NotificationService : Service
    {
        private const int ToDoCheckInterval = 3;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            var toDoNotificationService = ComponentFactory.Resolve<IToDoNotificationService>();

            Device.StartTimer(TimeSpan.FromSeconds(ToDoCheckInterval), () =>
            {
                toDoNotificationService.CheckForNotifications();
                return true;
            });
            return StartCommandResult.Sticky;
        }
    }
}