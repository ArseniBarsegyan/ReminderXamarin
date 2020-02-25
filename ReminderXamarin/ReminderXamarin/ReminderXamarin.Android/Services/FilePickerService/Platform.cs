using Android.App;
using Android.Content;

namespace ReminderXamarin.Droid.Services.FilePickerService
{
    public class Platform
    {
        private static MainActivity _mainActivity;
        private static int _requestCounter = 1;

        public static Activity MainActivity => _mainActivity;

        public static void Init(MainActivity mainActivity)
        {
            _mainActivity = mainActivity;
        }

        public static void StartActivityForResult(Intent intent, System.Action<Result, Intent> onResult)
        {
            int originalRequestCode;
            unchecked
            {
                if (_requestCounter < 0)
                {
                    _requestCounter = 1;
                }
                originalRequestCode = _requestCounter++;
            }
            System.Action<int, Result, Intent> listener = null;
            listener = (requestCode, resultCode, data) =>
            {
                if (originalRequestCode != requestCode)
                {
                    return;
                }
                onResult?.Invoke(resultCode, data);
                _mainActivity.ActivityResult -= listener;
            };
            _mainActivity.ActivityResult += listener;
            MainActivity.StartActivityForResult(intent, originalRequestCode);
        }
    }
}