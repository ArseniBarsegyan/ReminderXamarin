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

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MenuPage());
        }

        /// <summary>
        /// Gets the Note repository with help dependency service.
        /// </summary>
        /// <value>The database.</value>
        public static NoteRepository NoteRepository => _noteRepository ??
                                                 (_noteRepository = new NoteRepository(DependencyService.Get<IFileHelper>()
                                                     .GetLocalFilePath("MyDiaryDB.db3")));

        /// <summary>
        /// Gets the To-do models repository with help of dependency service.
        /// </summary>
        /// <value>The database.</value>
        public static ToDoRepository ToDoRepository => _toDoRepository ??
                                                       (_toDoRepository = new ToDoRepository(DependencyService.Get<IFileHelper>()
                                                           .GetLocalFilePath("MyDiaryDB.db3")));

        /// <summary>
        /// Gets the achievement models repository with help dependency service.
        /// </summary>
        /// <value>The database.</value>
        public static AchievementRepository AchievementRepository => _achievementRepository ??
                                                       (_achievementRepository = new AchievementRepository(DependencyService.Get<IFileHelper>()
                                                           .GetLocalFilePath("MyDiaryDB.db3")));

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
