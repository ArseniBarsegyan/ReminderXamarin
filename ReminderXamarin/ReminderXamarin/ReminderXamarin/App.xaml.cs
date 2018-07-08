﻿using ReminderXamarin.Helpers;
using ReminderXamarin.Interfaces;
using ReminderXamarin.Models;
using ReminderXamarin.Pages;
using Xamarin.Forms;

namespace ReminderXamarin
{
    public partial class App : Application
    {
        private static NoteRepository _noteRepository;
        private static ToDoRepository _toDoRepository;
        private static AchievementRepository _achievementRepository;
        private static UserRepository _userRepository;
        private static FriendsRepository _friendsRepository;

        public App()
        {
            InitializeComponent();
            MainPage = new LoginPage();
        }

        /// <summary>
        /// Get Note repository with help dependency service.
        /// </summary>
        /// <value>The database.</value>
        public static NoteRepository NoteRepository => _noteRepository ??
                                                 (_noteRepository = new NoteRepository(DependencyService.Get<IFileHelper>()
                                                     .GetLocalFilePath(ConstantHelper.SqLiteDataBaseName)));

        /// <summary>
        /// Get To-do models repository with help of dependency service.
        /// </summary>
        /// <value>The database.</value>
        public static ToDoRepository ToDoRepository => _toDoRepository ??
                                                       (_toDoRepository = new ToDoRepository(DependencyService.Get<IFileHelper>()
                                                           .GetLocalFilePath(ConstantHelper.SqLiteDataBaseName)));

        /// <summary>
        /// Get achievement models repository with help dependency service.
        /// </summary>
        /// <value>The database.</value>
        public static AchievementRepository AchievementRepository => _achievementRepository ??
                                                       (_achievementRepository = new AchievementRepository(DependencyService.Get<IFileHelper>()
                                                           .GetLocalFilePath(ConstantHelper.SqLiteDataBaseName)));

        /// <summary>
        /// Get users repository with help dependency service.
        /// </summary>
        /// <value>The database.</value>
        public static UserRepository UserRepository => _userRepository ??
                                                                     (_userRepository = new UserRepository(DependencyService.Get<IFileHelper>()
                                                                         .GetLocalFilePath(ConstantHelper.SqLiteDataBaseName)));

        /// <summary>
        /// Get friends repository with help dependency service.
        /// </summary>
        /// <value>The database.</value>
        public static FriendsRepository FriendsRepository => _friendsRepository ??
                                                       (_friendsRepository = new FriendsRepository(DependencyService.Get<IFileHelper>()
                                                           .GetLocalFilePath(ConstantHelper.SqLiteDataBaseName)));

        protected override void OnStart()
        {
            // Handle when your app starts
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
