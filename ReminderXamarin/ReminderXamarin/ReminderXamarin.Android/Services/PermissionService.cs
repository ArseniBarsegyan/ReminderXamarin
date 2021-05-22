using System;
using System.Threading.Tasks;

using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

using ReminderXamarin.Core.Interfaces;

namespace ReminderXamarin.Droid.Services
{
    public class PermissionService : IPermissionService
    {
        private static readonly int SdkVersion = (int)Android.OS.Build.VERSION.SdkInt;

        [Xamarin.Forms.Internals.Preserve]
        public PermissionService()
        {
        }
        
        public async Task<bool> AskPermission()
        {
            if (SdkVersion < 23)
            {
                return true;
            }

            try
            {
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);

                var cameraStatusResult = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera) ==
                                         PermissionStatus.Granted;
                var storageStatusResult = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage) ==
                                          PermissionStatus.Granted;

                return cameraStatusResult && storageStatusResult;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}