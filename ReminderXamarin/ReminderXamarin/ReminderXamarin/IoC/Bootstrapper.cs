using ReminderXamarin.Services.Navigation;
using ReminderXamarin.DependencyResolver;
using ReminderXamarin.Services;
using ReminderXamarin.Utilities;

namespace ReminderXamarin.IoC
{
    public static class Bootstrapper
    {
        public static bool IsInitialized { get; private set; }

        public static void Initialize()
        {
            if (!IsInitialized)
            {
                ComponentRegistry.Container = new SimpleInjectorContainerService();
                ComponentRegistry.Register<INavigationService, NavigationService>();
                ComponentRegistry.Register<IUploadService, UploadService>();
                ComponentRegistry.Register<ThemeSwitcher>();
                IsInitialized = true;
            }
        }
    }
}