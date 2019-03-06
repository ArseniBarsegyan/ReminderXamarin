using System;
using System.ComponentModel;
using System.Reflection;
using System.Resources;
using ReminderXamarin.Helpers;
using System.Threading.Tasks;

namespace ReminderXamarin.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public static readonly Lazy<ResourceManager> Resmgr = new Lazy<ResourceManager>(
            () => new ResourceManager(ConstantsHelper.TranslationResourcePath, typeof(BaseViewModel).GetTypeInfo().Assembly));

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}