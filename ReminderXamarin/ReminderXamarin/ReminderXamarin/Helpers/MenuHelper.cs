using System;
using System.Collections.Generic;
using ReminderXamarin.Views;
using ReminderXamarin.Resx;
using ReminderXamarin.Utilities;

namespace Rm.Helpers
{
    public static class MenuHelper
    {
        public static List<MasterPageItem> GetMenu(ThemeTypes themeType)
        {
            var masterPageItems = new List<MasterPageItem>
            {
                new MasterPageItem
                {
                    Title = AppResources.Notes,
                    IconSource = themeType == ThemeTypes.Dark
                        ? ConstantsHelper.NotesListLightIcon
                        : ConstantsHelper.NotesListDarkIcon,
                    TargetType = typeof(NotesView),
                    IsDisplayed = true,
                    Index = MenuViewIndex.NotesView
                },
                new MasterPageItem
                {
                    Title = AppResources.ToDoSection,
                    IconSource = themeType == ThemeTypes.Dark
                        ? ConstantsHelper.ToDoListLightIcon
                        : ConstantsHelper.ToDoListDarkIcon,
                    TargetType = typeof(ToDoTabbedView),
                    IsDisplayed = true,
                    Index = MenuViewIndex.ToDoPage
                },
                new MasterPageItem
                {
                    Title = AppResources.Birthdays,
                    IconSource = themeType == ThemeTypes.Dark
                        ? ConstantsHelper.BirthdaysIcon
                        : ConstantsHelper.BirthdaysDarkIcon,
                    TargetType = typeof(BirthdaysView),
                    IsDisplayed = true,
                    Index = MenuViewIndex.BirthdaysView
                },
                new MasterPageItem
                {
                    Title = AppResources.Achievements,
                    IconSource = themeType == ThemeTypes.Dark
                        ? ConstantsHelper.AchievementIcon
                        : ConstantsHelper.AchievementDarkIcon,
                    TargetType = typeof(AchievementsView),
                    IsDisplayed = true,
                    Index = MenuViewIndex.AchievementsView
                },
                new MasterPageItem
                {
                    Title = AppResources.Settings,
                    IconSource = themeType == ThemeTypes.Dark
                        ? ConstantsHelper.SettingsLightIcon
                        : ConstantsHelper.SettingsDarkIcon,
                    TargetType = typeof(SettingsView),
                    IsDisplayed = true,
                    Index = MenuViewIndex.SettingsView
                }
            };
            return masterPageItems;
        }
    }

    public class MasterPageItem
    {
        public string Title { get; set; }
        public string IconSource { get; set; }
        public Type TargetType { get; set; }
        public bool IsDisplayed { get; set; }
        public MenuViewIndex Index { get; set; }
    }

    public enum MenuViewIndex
    {
        NotesView,
        ToDoPage,
        BirthdaysView,
        AchievementsView,
        SettingsView
    }
}