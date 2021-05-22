using System;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;

using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

using ReminderXamarin.Services.Navigation;

using Rm.Helpers;

namespace ReminderXamarin.ViewModels.Base
{
    public class BaseNavigableViewModel : BaseViewModel
    {
        protected readonly INavigationService NavigationService;

        public BaseNavigableViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public static readonly Lazy<ResourceManager> Resmgr = new Lazy<ResourceManager>(
            () => new ResourceManager(ConstantsHelper.TranslationResourcePath,
                typeof(BaseNavigableViewModel).GetTypeInfo().Assembly));

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }

        protected async Task<bool> CheckPermissionsAsync()
        {
            var result = false;
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                    {
                        await Acr.UserDialogs.UserDialogs.Instance.AlertAsync(ConstantsHelper.StoragePermissionRequiredMessage,
                            ConstantsHelper.Warning);
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                }

                if (status == PermissionStatus.Granted)
                {
                    result = true;

                }
                else if (status != PermissionStatus.Unknown)
                {
                    await Acr.UserDialogs.UserDialogs.Instance.AlertAsync(ConstantsHelper.PermissionDeniedMessage,
                        ConstantsHelper.Warning);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await Acr.UserDialogs.UserDialogs.Instance.AlertAsync(ConstantsHelper.CantContinueErrorMessage,
                    ConstantsHelper.Warning);
            }
            return result;
        }
    }
}