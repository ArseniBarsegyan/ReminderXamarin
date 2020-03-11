using System;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

using ReminderXamarin.Core.Interfaces;
using ReminderXamarin.DependencyResolver;
using ReminderXamarin.IoC;
using ReminderXamarin.Services;
using ReminderXamarin.Services.MediaPicker;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.Utilities;
using ReminderXamarin.ViewModels;

using Rm.Data.Data.Repositories;
using Rm.Helpers;

using Xamarin.Forms;

namespace ReminderXamarin
{
    public partial class App : Application
    {
        private readonly ThemeSwitcher _themeSwitchService;
        private readonly IThemeService _themeService;
        private readonly INavigationService _navigationService;

        public static IMultiMediaPickerService MultiMediaPickerService;

        public App(IMultiMediaPickerService multiMediaPickerService)
        {
            InitializeComponent();

            Bootstrapper.RegisterServices();
            InitRepositories();
            MultiMediaPickerService = multiMediaPickerService;                   

            _navigationService = ComponentFactory.Resolve<INavigationService>();
            _themeSwitchService = ComponentFactory.Resolve<ThemeSwitcher>();
            _themeService = ComponentFactory.Resolve<IThemeService>();            

            bool.TryParse(Settings.UsePin, out var shouldUsePin);
            InitNavigation(shouldUsePin);
        }

        private void InitRepositories()
        {
            var dbPath = ComponentFactory.Resolve<IFileHelper>().GetLocalFilePath(ConstantsHelper.SqLiteDataBaseName);

            NoteRepository = new Lazy<NoteRepository>(() => new NoteRepository(dbPath));
            ToDoRepository = new Lazy<ToDoRepository>(() => new ToDoRepository(dbPath));
            AchievementRepository = new Lazy<AchievementRepository>(() => new AchievementRepository(dbPath));
            UserRepository = new Lazy<UserRepository>(() => new UserRepository(dbPath));
            BirthdaysRepository = new Lazy<BirthdaysRepository>(() => new BirthdaysRepository(dbPath));
            AchievementStepRepository = new Lazy<AchievementStepRepository>(() => new AchievementStepRepository(dbPath));
        }

        public static int ScreenWidth { get; set; }
        public static int ScreenHeight { get; set; }

        public static Lazy<NoteRepository> NoteRepository { get; private set; }
        public static Lazy<ToDoRepository> ToDoRepository { get; private set; }
        public static Lazy<AchievementRepository> AchievementRepository { get; private set; }
        public static Lazy<UserRepository> UserRepository { get; private set; }
        public static Lazy<BirthdaysRepository> BirthdaysRepository { get; private set; }
        public static Lazy<AchievementStepRepository> AchievementStepRepository { get; private set; }

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
            _themeSwitchService.Reset();
        }

        protected override void OnResume()
        {
        }

        private void InitNavigation(bool shouldUsePin)
        {
            _themeSwitchService.InitializeTheme();
            _themeService.SetStatusBarColor((Color)Current.Resources["StatusBar"]);

            if (shouldUsePin)
            {
                _navigationService.ToRootAsync<PinViewModel>();
            }
            else
            {
                _navigationService.ToRootAsync<LoginViewModel>();
            }
        }
    }
}
