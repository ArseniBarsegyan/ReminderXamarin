using Plugin.LocalNotifications;

using ReminderXamarin.Enums;

using Rm.Helpers;

using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.Services
{
    public class ToDoNotificationsService : IToDoNotificationService
    {
        [Preserve]
        public ToDoNotificationsService()
        {
        }
        
        public void CheckForNotifications()
        {
            var currentDate = DateTime.Now;

            var allModels = App.ToDoRepository.Value.GetAll()
                .Where(x => x.UserId == Settings.CurrentUserId);

            var allModelsForNotifications = App.ToDoRepository.Value.GetAll()
                .Where(x => x.UserId == Settings.CurrentUserId
                        && x.Status == ConstantsHelper.Active
                        && x.WhenHappens.ToString("dd.MM.yyyy") == currentDate.ToString()
                        && (x.WhenHappens.Hour - currentDate.Hour <= Math.Abs(2)))
                .ToList();

            foreach (var model in allModelsForNotifications)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    CrossLocalNotifications.Current.Show(model.WhenHappens.ToString("D"), model.Description);
                });

                model.Status = ToDoStatus.Completed.ToString();
                App.ToDoRepository.Value.Save(model);
                MessagingCenter.Send((App)App.Current, ConstantsHelper.UpdateUI);
            }
        }
    }
}
