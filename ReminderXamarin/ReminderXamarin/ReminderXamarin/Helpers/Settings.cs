using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Rm.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private const string CurrentUserIdKey = "CurrentIdKey";
        private const string UsePinKey = "UsePinCode";
        private const string UserPinCodeKey = "UserPinCode";
        private const string ApplicationUserKey = "ApplicationUserKey";
        private const string ThemeTypeKey = "CurrentThemeKey";
        private const string UseSafeModeKey = "UseSafeModeKey";
        private const string PinBackgroundKey = "PinBackgroundKey";
        private const string UsePinBackgroundKey = "UsePinBackground";

        private static readonly string SettingsDefault = string.Empty;
        private static readonly string ThemeSettingDefault = "Light";

        #endregion

        public static string CurrentUserId
        {
            get => AppSettings.GetValueOrDefault(CurrentUserIdKey, SettingsDefault);
            set => AppSettings.AddOrUpdateValue(CurrentUserIdKey, value);
        }

        public static string UsePin
        {
            get => AppSettings.GetValueOrDefault(UsePinKey, SettingsDefault);
            set => AppSettings.AddOrUpdateValue(UsePinKey, value);
        }

        public static string UserPinCode
        {
            get => AppSettings.GetValueOrDefault(UserPinCodeKey, SettingsDefault);
            set => AppSettings.AddOrUpdateValue(UserPinCodeKey, value);
        }

        public static string ApplicationUser
        {
            get => AppSettings.GetValueOrDefault(ApplicationUserKey, SettingsDefault);
            set => AppSettings.AddOrUpdateValue(ApplicationUserKey, value);
        }

        public static string GeneralSettings
        {
            get => AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            set => AppSettings.AddOrUpdateValue(SettingsKey, value);
        }

        public static string ThemeType
        {
            get => AppSettings.GetValueOrDefault(ThemeTypeKey, ThemeSettingDefault);
            set => AppSettings.AddOrUpdateValue(ThemeTypeKey, value);
        }

        public static string UseSafeMode
        {
            get => AppSettings.GetValueOrDefault(UseSafeModeKey, SettingsDefault);
            set => AppSettings.AddOrUpdateValue(UseSafeModeKey, value);
        }
        
        public static string PinBackground
        {
            get => AppSettings.GetValueOrDefault(PinBackgroundKey, SettingsDefault);
            set => AppSettings.AddOrUpdateValue(PinBackgroundKey, value);
        }

        public static string UsePinBackground
        {
            get => AppSettings.GetValueOrDefault(UsePinBackgroundKey, SettingsDefault);
            set => AppSettings.AddOrUpdateValue(UsePinBackgroundKey, value);
        }

        public static void Clear()
        {
            AppSettings.Clear();
        }
    }
}