using System;

using ReminderXamarin.ResourceDictionaries;
using Rm.Helpers;
using Xamarin.Forms;

namespace ReminderXamarin.Utilities
{
    public class ThemeSwitcher
    {
        public ThemeTypes CurrentThemeType { get; private set; }
        private readonly LightThemeDictionary _lightThemeDictionary;
        private readonly DarkThemeDictionary _darkThemeDictionary;

        public ThemeSwitcher()
        {
            _lightThemeDictionary = new LightThemeDictionary();
            _darkThemeDictionary = new DarkThemeDictionary();
        }

        public void InitializeTheme()
        {
            SwitchTheme(GetThemeType());
        }

        public void Reset()
        {
            CurrentThemeType = ThemeTypes.Light;
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
                }
                CurrentThemeType = themeType;
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
