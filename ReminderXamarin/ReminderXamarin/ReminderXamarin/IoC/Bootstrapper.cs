using ReminderXamarin.Commanding;
using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.DependencyResolver;
using ReminderXamarin.Services;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.Utilities;

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
            ComponentRegistry.Register<ThemeSwitcher>();

            ComponentRegistry.Register<IAsyncCommand, AsyncCommand>();
            ComponentRegistry.Register<ICommandExecutionLock, CommandExecutionLock>();
            ComponentRegistry.Register<ICommandResolver, CommandResolver>();
        }
    }
}