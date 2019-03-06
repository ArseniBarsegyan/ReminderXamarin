using System;
using System.ComponentModel;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using ReminderXamarin.Helpers;
using ReminderXamarin.Interfaces.Navigation;
using RmApp.Core.DependencyResolver;

namespace ReminderXamarin.ViewModels.Base
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected readonly INavigationService NavigationService;

        protected BaseViewModel()
        {
            NavigationService = ComponentFactory.Resolve<INavigationService>();
        }

        public static readonly Lazy<ResourceManager> Resmgr = new Lazy<ResourceManager>(
            () => new ResourceManager(ConstantsHelper.TranslationResourcePath, typeof(BaseViewModel).GetTypeInfo().Assembly));

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}