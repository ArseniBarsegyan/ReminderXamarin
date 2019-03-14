using System;
using System.ComponentModel;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReminderXamarin.Data.EF;
using ReminderXamarin.Data.Entities;
using ReminderXamarin.Data.Repositories;
using ReminderXamarin.DependencyResolver;
using ReminderXamarin.Helpers;
using ReminderXamarin.Services;
using ReminderXamarin.Services.Navigation;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels.Base
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected readonly INavigationService NavigationService;

        protected BaseViewModel()
        {
            NavigationService = ComponentFactory.Resolve<INavigationService>();

            string dbPath = DependencyService.Get<IFileHelper>().GetLocalFilePath(ConstantsHelper.EFConnectionString);

            var options = new DbContextOptionsBuilder<AppIdentityDbContext>();
            options.UseSqlite($"Filename={dbPath}");
            
            var context = new AppIdentityDbContext(options.Options,
                "dbo",
                dbPath);

            NoteRepository = new Lazy<EntityFrameworkRepository<AppIdentityDbContext, Note>>(() =>
                new EntityFrameworkRepository<AppIdentityDbContext, Note>(context));
            ToDoRepository = new Lazy<EntityFrameworkRepository<AppIdentityDbContext, ToDoModel>>(() =>
                new EntityFrameworkRepository<AppIdentityDbContext, ToDoModel>(context));
            AchievementRepository = new Lazy<EntityFrameworkRepository<AppIdentityDbContext, AchievementModel>>(() =>
                new EntityFrameworkRepository<AppIdentityDbContext, AchievementModel>(context));
            UserRepository = new Lazy<EntityFrameworkRepository<AppIdentityDbContext, AppUser>>(() =>
                new EntityFrameworkRepository<AppIdentityDbContext, AppUser>(context));
            BirthdaysRepository = new Lazy<EntityFrameworkRepository<AppIdentityDbContext, BirthdayModel>>(() =>
                new EntityFrameworkRepository<AppIdentityDbContext, BirthdayModel>(context));
        }

        protected static Lazy<EntityFrameworkRepository<AppIdentityDbContext, Note>> NoteRepository;
        protected static Lazy<EntityFrameworkRepository<AppIdentityDbContext, ToDoModel>> ToDoRepository;
        protected static Lazy<EntityFrameworkRepository<AppIdentityDbContext, AchievementModel>> AchievementRepository;
        protected static Lazy<EntityFrameworkRepository<AppIdentityDbContext, AppUser>> UserRepository;
        protected static Lazy<EntityFrameworkRepository<AppIdentityDbContext, BirthdayModel>> BirthdaysRepository;

        public static readonly Lazy<ResourceManager> Resmgr = new Lazy<ResourceManager>(
            () => new ResourceManager(ConstantsHelper.TranslationResourcePath, typeof(BaseViewModel).GetTypeInfo().Assembly));

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}