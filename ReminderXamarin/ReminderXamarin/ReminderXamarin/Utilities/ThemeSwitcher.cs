using ReminderXamarin.ResourceDictionaries;

using Rm.Helpers;

using System;

using Xamarin.Forms;

namespace ReminderXamarin.Utilities
{
    public class ThemeSwitcher
    {       
        private readonly LightThemeDictionary _lightThemeDictionary;
        private readonly DarkThemeDictionary _darkThemeDictionary;

        public ThemeTypes CurrentThemeType { get; private set; }

        public ThemeSwitcher()
        {
            _lightThemeDictionary = new LightThemeDictionary();
            _darkThemeDictionary = new DarkThemeDictionary();
        }

        public void InitializeTheme()
        {
            SwitchTheme(GetThemeType());
        }

        public void SwitchTheme(ThemeTypes themeType)
        {
            if (themeType != CurrentThemeType)
            {
                Application.Current.Resources.MergedDictionaries.Clear();                

                switch (themeType)
                {
                    case ThemeTypes.Light:
                        {
                            Application.Current.Resources.MergedDictionaries.Add(_lightThemeDictionary);
                            break;
                        }
                    case ThemeTypes.Dark:
                        {
                            Application.Current.Resources.MergedDictionaries.Add(_darkThemeDictionary);
                            break;
                        }
                    case ThemeTypes.Default:
                        {
                            Application.Current.Resources.MergedDictionaries.Add(_lightThemeDictionary);
                            break;
                        }
                }
                CurrentThemeType = themeType;
                MessagingCenter.Send(this, ConstantsHelper.AppThemeChanged);
            }
        }

        public ThemeTypes GetThemeType()
        {
            if (string.IsNullOrEmpty(Settings.ThemeType))
            {
                return ThemeTypes.Light;
            }
            return (ThemeTypes)Enum.Parse(typeof(ThemeTypes), Settings.ThemeType);
        }
    }
}
