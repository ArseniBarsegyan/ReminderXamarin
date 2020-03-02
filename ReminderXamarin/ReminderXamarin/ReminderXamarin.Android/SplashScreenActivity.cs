using System.Threading.Tasks;

using Android.Animation;
using Android.App;
using Android.OS;

using Com.Airbnb.Lottie;

namespace ReminderXamarin.Droid
{
    [Activity(Theme = "@style/Reminder.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashScreenActivity : Activity, Animator.IAnimatorListener
    {
        private LottieAnimationView _animationView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.launch_fragment_layout);
            _animationView = FindViewById<LottieAnimationView>(Resource.Id.SpinnerView);
            _animationView.AddAnimatorListener(this);

            Task.Run(async() =>
            {
                await Task.Delay(1000);
                StartActivity(typeof(MainActivity));
            });
        }

        public void OnAnimationCancel(Animator animation)
        {
        }

        public void OnAnimationEnd(Animator animation)
        {
        }

        public void OnAnimationRepeat(Animator animation)
        {
        }

        public void OnAnimationStart(Animator animation)
        {
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (_animationView == null)
            {
                _animationView = FindViewById<LottieAnimationView>(Resource.Id.SpinnerView);
            }
            _animationView.AddAnimatorListener(this);
        }
    }
}