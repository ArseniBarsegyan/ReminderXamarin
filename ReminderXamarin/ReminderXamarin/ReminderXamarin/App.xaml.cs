﻿using System;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using Realms;
using ReminderXamarin.DependencyResolver;
using ReminderXamarin.Helpers;
using ReminderXamarin.Services;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.IoC;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using RI.Data.Data.Repositories;

namespace ReminderXamarin
{
    public partial class App : Application
    {
        private readonly INavigationService _navigationService;
        public const string NotificationReceivedKey = "NotificationReceived";

        public App()
        {
            var time = DateTime.Now;

            InitializeComponent();
            Bootstrapper.Initialize();
            _navigationService= ComponentFactory.Resolve<INavigationService>();

            string dbPath = DependencyService.Get<IFileHelper>().GetLocalFilePath(ConstantsHelper.EFConnectionString);

            //var options = new DbContextOptionsBuilder<AppIdentityDbContext>();
            //options.UseSqlite($"Filename={dbPath}");

            var contextTimeMark = DateTime.Now;
            TimeSpan difference = DateTime.Now - time;
            Console.WriteLine($"Warning: Warning '!!!!!!!!!!!!!!!!!!Before context creation: '" + difference.TotalMilliseconds);

            //var context = new AppIdentityDbContext(options.Options, 
            //    "dbo",
            //    dbPath);

            DateTime repoCreationTimeMark = DateTime.Now;
            difference = repoCreationTimeMark - contextTimeMark;
            Console.WriteLine("Warning: Warning '!!!!!!!!!!!!!!!!!!After context creation:' " + difference.TotalMilliseconds);

            //NoteRepository = new EntityFrameworkRepository<AppIdentityDbContext, Note>(context);
            //ToDoRepository = new EntityFrameworkRepository<AppIdentityDbContext, ToDoModel>(context);
            //AchievementRepository = new EntityFrameworkRepository<AppIdentityDbContext, AchievementModel>(context);
            //UserRepository = new EntityFrameworkRepository<AppIdentityDbContext, AppUser>(context);
            //BirthdaysRepository = new EntityFrameworkRepository<AppIdentityDbContext, BirthdayModel>(context);

            var realmInstance = Realm.GetInstance(dbPath);

            NoteRepository = new RealmRepository<RI.Data.Data.Entities.Note>(realmInstance);
            ToDoRepository = new RealmRepository<RI.Data.Data.Entities.ToDoModel>(realmInstance);
            AchievementRepository = new RealmRepository<RI.Data.Data.Entities.AchievementModel>(realmInstance);
            UserRepository = new RealmRepository<RI.Data.Data.Entities.AppUser>(realmInstance);
            BirthdaysRepository = new RealmRepository<RI.Data.Data.Entities.BirthdayModel>(realmInstance);

            bool.TryParse(Settings.UsePin, out bool shouldUsePin);
            if (shouldUsePin)
            {
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

        //public static EntityFrameworkRepository<AppIdentityDbContext, Note> NoteRepository { get; private set; }
        //public static EntityFrameworkRepository<AppIdentityDbContext, ToDoModel> ToDoRepository { get; private set; }
        //public static EntityFrameworkRepository<AppIdentityDbContext, AchievementModel> AchievementRepository { get; private set; }
        //public static EntityFrameworkRepository<AppIdentityDbContext, AppUser> UserRepository { get; private set; }
        //public static EntityFrameworkRepository<AppIdentityDbContext, BirthdayModel> BirthdaysRepository { get; private set; }

        public static RealmRepository<RI.Data.Data.Entities.Note> NoteRepository { get; private set; }
        public static RealmRepository<RI.Data.Data.Entities.ToDoModel> ToDoRepository { get; private set; }
        public static RealmRepository<RI.Data.Data.Entities.AchievementModel> AchievementRepository { get; private set; }
        public static RealmRepository<RI.Data.Data.Entities.AppUser> UserRepository { get; private set; }
        public static RealmRepository<RI.Data.Data.Entities.BirthdayModel> BirthdaysRepository { get; private set; }

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
