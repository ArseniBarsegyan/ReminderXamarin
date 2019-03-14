using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using ReminderXamarin.DependencyResolver;
using ReminderXamarin.Helpers;
using ReminderXamarin.Services;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.IoC;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;

namespace ReminderXamarin
{
    public partial class App : Application
    {
        //private readonly AppIdentityDbContext _dbContext;
        private readonly INavigationService _navigationService;
        public const string NotificationReceivedKey = "NotificationReceived";

        public App()
        {
            InitializeComponent();
            Bootstrapper.Initialize();
            _navigationService= ComponentFactory.Resolve<INavigationService>();

            bool.TryParse(Settings.UsePin, out bool shouldUsePin);
            if (shouldUsePin)
            {
                // MainPage = new PinView();
                _navigationService.InitializeAsync<PinViewModel>();
            }
            else
            {
                _navigationService.InitializeAsync<LoginViewModel>();
            }

            MessagingCenter.Subscribe<object, string>(this, NotificationReceivedKey, OnMessageReceived);
        }

        // TODO: implement notification logic here
        private void OnMessageReceived(object sender, string msg)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                var alertService = DependencyService.Get<IAlertService>();
                alertService.ShowYesNoAlert("Notification received", "Ok", "Cancel");
            });
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            AppCenter.Start("dbcc1105-ebfa-4b6a-8fec-8ea02bd5454e", typeof(Push));
            AppCenter.Start("android=dbcc1105-ebfa-4b6a-8fec-8ea02bd5454e;" 
                + "uwp={Your UWP App secret here};"
                + "ios={Your iOS App secret here}", 
                typeof(Analytics), 
                typeof(Crashes));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
