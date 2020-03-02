using System.Threading.Tasks;

using Android.App;
using Android.Support.V4.Content;
using Android.Widget;

using Plugin.CurrentActivity;

using ReminderXamarin.Services;

namespace ReminderXamarin.Droid.Services
{
    public class AlertService : IAlertService
    {
        public Task<bool> ShowYesNoAlert(string message, string yesButtonText, string noButtonText)
        {
            var tcs = new TaskCompletionSource<bool>();
            var dialog = new Dialog(CrossCurrentActivity.Current.Activity);
            dialog.Window.SetBackgroundDrawable(ContextCompat.GetDrawable(CrossCurrentActivity.Current.Activity, Android.Resource.Color.Transparent));
            dialog.SetContentView(Resource.Layout.custom_alert_yes_no);
            TextView text = (TextView)dialog.FindViewById(Resource.Id.yesNoText);
            text.SetText(message, TextView.BufferType.Normal);

            Button acceptButton = (Button)dialog.FindViewById(Resource.Id.yesNoApplyButton);
            acceptButton.SetText(yesButtonText, TextView.BufferType.Normal);

            dialog.DismissEvent += (sender, e) =>
            {
                tcs.TrySetResult(false);
            };

            acceptButton.Click += (sender, e) =>
            {
                tcs.TrySetResult(true);
                dialog.Dismiss();
            };

            Button rejectButton = (Button)dialog.FindViewById(Resource.Id.yesNoRejectButton);
            rejectButton.SetText(noButtonText, TextView.BufferType.Normal);
            rejectButton.Click += (sender, e) =>
            {
                tcs.TrySetResult(false);
                dialog.Dismiss();
            };

            dialog.Show();

            return tcs.Task;
        }
    }
}