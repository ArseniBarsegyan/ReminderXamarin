using System;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Rm.Data.Data.Repositories;
using ReminderXamarin.DependencyResolver;
using Rm.Helpers;
using ReminderXamarin.Services;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.IoC;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;

namespace ReminderXamarin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Bootstrapper.Initialize();
            var navigationService = ComponentFactory.Resolve<INavigationService>();
            var dbPath = DependencyService.Get<IFileHelper>().GetLocalFilePath(ConstantsHelper.SqLiteDataBaseName);

            NoteRepository = new Lazy<NoteRepository>(() => new NoteRepository(dbPath));
            ToDoRepository = new Lazy<ToDoRepository>(() => new ToDoRepository(dbPath));
            AchievementRepository = new Lazy<AchievementRepository>(() => new AchievementRepository(dbPath));
            UserRepository = new Lazy<UserRepository>(() => new UserRepository(dbPath));
            BirthdaysRepository = new Lazy<BirthdaysRepository>(() => new BirthdaysRepository(dbPath));

            bool.TryParse(Settings.UsePin, out var shouldUsePin);
            if (shouldUsePin)
            {
                navigationService.InitializeAsync<PinViewModel>();
            }
            else
            {
                navigationService.InitializeAsync<LoginViewModel>();
            }
        }

        public static int ScreenWidth { get; set; }
        public static int ScreenHeight { get; set; }

        public static Lazy<NoteRepository> NoteRepository { get; private set; }
        public static Lazy<ToDoRepository> ToDoRepository { get; private set; }
        public static Lazy<AchievementRepository> AchievementRepository { get; private set; }
        public static Lazy<UserRepository> UserRepository { get; private set; }
        public static Lazy<BirthdaysRepository> BirthdaysRepository { get; private set; }

        protected override void OnStart()
        {
            // Handle when your app starts
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
