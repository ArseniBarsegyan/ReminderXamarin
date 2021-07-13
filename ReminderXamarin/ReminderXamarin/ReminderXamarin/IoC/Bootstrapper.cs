using ReminderXamarin.Commanding;
using ReminderXamarin.Core.Interfaces;
using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Core.Interfaces.Services;
using ReminderXamarin.DependencyResolver;
using ReminderXamarin.Helpers;
using ReminderXamarin.Services;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.Utilities;

using Rm.Helpers;

namespace ReminderXamarin.IoC
{
    public static class Bootstrapper
    {
        public static void Initialize()
        {
            ComponentRegistry.Container = new SimpleInjectorContainerService();            
        }

        public static void RegisterServices()
        {
            ComponentRegistry.Register<INavigationService, NavigationService>();
            ComponentRegistry.Register<IUploadService, UploadService>();
            ComponentRegistry.Register<IToDoNotificationService, ToDoNotificationsService>();
            ComponentRegistry.Register<ThemeSwitcher>();

            ComponentRegistry.Register<IAsyncCommand, AsyncCommand>();
            ComponentRegistry.Register<ICommandExecutionLock, CommandExecutionLock>();
            ComponentRegistry.Register<ICommandResolver, CommandResolver>();

            ComponentRegistry.Register<TransformHelper>();
            ComponentRegistry.Register<MediaHelper>();
        }
    }
}