using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Plugin.LocalNotifications;
using ReminderXamarin.Enums;
using Rm.Helpers;
using Xamarin.Forms;

namespace ReminderXamarin.Droid
{
    [Service]
    public class NotificationService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                var currentDate = DateTime.Now;

                var allToDoModels = App.ToDoRepository.Value.GetAll()
                    .Where(x => x.Status == ConstantsHelper.Active)
                    .Where(x => x.WhenHappens.ToString("dd.MM.yyyy HH:mm") == currentDate.ToString("dd.MM.yyyy HH:mm"))
                    .ToList();

                allToDoModels.ForEach(model =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        CrossLocalNotifications.Current.Show(model.WhenHappens.ToString("D"), model.Description);
                    });
                    model.Status = ToDoStatus.Completed.ToString();
                    App.ToDoRepository.Value.Save(model);
                    MessagingCenter.Send((App)App.Current, ConstantsHelper.UpdateUI);
                });
                return true;
            });
            return StartCommandResult.Sticky;
        }
    }
}