using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Rm.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private const string CurrentUserIdKey = "0";
        private const string UsePinKey = "True";
        private const string UserPinCodeKey = "0000";
        private const string ApplicationUserKey = "";
        private const string ThemeTypeKey = "Light";
        private const string UseSafeModeKey = "False";

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

        public static void Clear()
        {
            AppSettings.Clear();
        }
    }
}