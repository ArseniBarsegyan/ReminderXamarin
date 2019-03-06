using ReminderXamarin.Interfaces.Navigation;
using RmApp.Core.DependencyResolver;
using RmApp.Core.Services.IoC;

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
                IsInitialized = true;
            }
        }
    }
}