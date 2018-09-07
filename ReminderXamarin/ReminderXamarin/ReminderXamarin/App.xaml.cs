using ReminderXamarin.EF;
using ReminderXamarin.EF.Repositories;
using ReminderXamarin.Helpers;
using ReminderXamarin.Interfaces;
using ReminderXamarin.Pages;
using Xamarin.Forms;

namespace ReminderXamarin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            string dbPath = DependencyService.Get<IFileHelper>().GetLocalFilePath(ConstantHelper.SqLiteDataBaseName);
            ApplicationContext context = new ApplicationContext(dbPath);

            NoteRepository = new NoteRepository(context);
            ToDoRepository = new ToDoRepository(context);
            AchievementRepository = new AchievementRepository(context);
            UserRepository = new UserRepository(context);
            BirthdaysRepository = new BirthdaysRepository(context);

            bool.TryParse(Settings.UsePin, out bool shouldUsePin);
            if (shouldUsePin)
            {
                MainPage = new PinPage();
            }
            else
            {
                MainPage = new LoginPage();
            }
        }

        public static NoteRepository NoteRepository { get; private set; }
        public static ToDoRepository ToDoRepository { get; private set; }
        public static AchievementRepository AchievementRepository { get; private set; }
        public static UserRepository UserRepository { get; private set; }
        public static BirthdaysRepository BirthdaysRepository { get; private set; }

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
