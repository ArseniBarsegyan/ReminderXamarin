using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using Microsoft.EntityFrameworkCore;
using ReminderXamarin.Helpers;
using ReminderXamarin.Interfaces;
using ReminderXamarin.Pages;
using Rm.Data.EF;
using Rm.Data.Entities;
using Rm.Data.Repositories;
using Xamarin.Forms;
using AchievementModel = Rm.Data.Entities.AchievementModel;
using BirthdayModel = Rm.Data.Entities.BirthdayModel;
using Device = Xamarin.Forms.Device;
using Note = Rm.Data.Entities.Note;
using ToDoModel = Rm.Data.Entities.ToDoModel;

namespace ReminderXamarin
{
    public partial class App : Application
    {
        public const string NotificationReceivedKey = "NotificationReceived";

        public App()
        {
            InitializeComponent();
            string dbPath = DependencyService.Get<IFileHelper>().GetLocalFilePath(ConstantsHelper.EFConnectionString);

            var options = new DbContextOptionsBuilder<AppIdentityDbContext>();
            options.UseSqlite($"Filename={dbPath}");

            var context = new AppIdentityDbContext(options.Options, 
                "dbo",
                dbPath);

            NoteRepository = new EntityFrameworkRepository<AppIdentityDbContext, Note>(context);
            ToDoRepository = new EntityFrameworkRepository<AppIdentityDbContext, ToDoModel>(context);
            AchievementRepository = new EntityFrameworkRepository<AppIdentityDbContext, AchievementModel>(context);
            UserRepository = new EntityFrameworkRepository<AppIdentityDbContext, AppUser>(context);
            BirthdaysRepository = new EntityFrameworkRepository<AppIdentityDbContext, BirthdayModel>(context);

            bool.TryParse(Settings.UsePin, out bool shouldUsePin);
            if (shouldUsePin)
            {
                MainPage = new PinPage();
            }
            else
            {
                MainPage = new LoginPage();
            }

            MessagingCenter.Subscribe<object, string>(this, NotificationReceivedKey, OnMessageReceived);
        }

        // TODO: implement notification logic here
        private void OnMessageReceived(object sender, string msg)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var alertService = DependencyService.Get<IAlertService>();
                alertService.ShowYesNoAlert("Notification received", "Ok", "Cancel");
            });
        }

        public static EntityFrameworkRepository<AppIdentityDbContext, Note> NoteRepository { get; private set; }
        public static EntityFrameworkRepository<AppIdentityDbContext, ToDoModel> ToDoRepository { get; private set; }
        public static EntityFrameworkRepository<AppIdentityDbContext, AchievementModel> AchievementRepository { get; private set; }
        public static EntityFrameworkRepository<AppIdentityDbContext, AppUser> UserRepository { get; private set; }
        public static EntityFrameworkRepository<AppIdentityDbContext, BirthdayModel> BirthdaysRepository { get; private set; }
        
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
