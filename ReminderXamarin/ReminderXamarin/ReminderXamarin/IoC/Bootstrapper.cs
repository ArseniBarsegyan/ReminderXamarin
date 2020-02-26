using ReminderXamarin.DependencyResolver;
using ReminderXamarin.Services;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.Utilities;
using ReminderXamarin.ViewModels;

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
        }
    }
}