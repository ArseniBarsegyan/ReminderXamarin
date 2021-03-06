﻿using Foundation;
using ReminderXamarin.Core.Interfaces;

using UIKit;

namespace ReminderXamarin.iOS.Services
{
    public class DeviceOrientation : IDeviceOrientation
    {
        [Preserve]
        public DeviceOrientation()
        {
        }
        
        public DeviceOrientations GetOrientation()
        {
            var currentOrientation = UIApplication.SharedApplication.StatusBarOrientation;
            bool isPortrait = currentOrientation == UIInterfaceOrientation.Portrait
                              || currentOrientation == UIInterfaceOrientation.PortraitUpsideDown;

            return isPortrait ? DeviceOrientations.Portrait : DeviceOrientations.Landscape;
        }
    }
}