using System;
using System.ComponentModel;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using ReminderXamarin.DependencyResolver;
using Rm.Helpers;
using ReminderXamarin.Services.Navigation;

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

        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }

        protected async Task<bool> CheckPermissionsAsync()
        {
            var retVal = false;
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                    {
                        await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Need Storage permission to access to your photos.",
                            "Alert");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] {Permission.Storage });
                    status = results[Permission.Storage];
                }

                if (status == PermissionStatus.Granted)
                {
                    retVal = true;

                }
                else if (status != PermissionStatus.Unknown)
                {
                    await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Permission Denied. Can not continue, try again.",
                        "Alert");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Error. Can not continue, try again.",
                    "Alert");
            }
            return retVal;
        }
    }
}