﻿using System;
using System.Threading.Tasks;
using Foundation;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

using ReminderXamarin.Core.Interfaces;

namespace ReminderXamarin.iOS.Services
{
    public class PermissionService : IPermissionService
    {
        [Preserve]
        public PermissionService()
        {
        }
        
        public async Task<bool> AskPermission()
        {
            try
            {
                //Configure required permissions here and include them into info.plist
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