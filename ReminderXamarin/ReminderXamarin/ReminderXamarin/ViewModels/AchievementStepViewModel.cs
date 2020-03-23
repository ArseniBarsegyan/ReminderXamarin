using System;
using System.Threading.Tasks;

using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

using Rm.Data.Data.Entities;
using Rm.Helpers;

using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class AchievementStepViewModel : BaseViewModel
    {
        private bool _isEnabled;
        private AchievementStep _model;

        public AchievementStepViewModel(
            INavigationService navigationService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            SaveStepCommand = commandResolver.AsyncCommand(SaveStep, () => { return IsEnabled; });
            NavigateBackCommand = commandResolver.AsyncCommand(NavigateBack);
        }
                
        public string Title { get; set; }
        public string NotesText { get; set; }
        public string TimeSpent { get; set; }
        public bool IsEnabled
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Title)
                    || string.IsNullOrWhiteSpace(NotesText)
                    || string.IsNullOrWhiteSpace(TimeSpent))
                {
                    return false;
                }
                if (Title.Length < 3 
                    || NotesText.Length < 3)
                {
                    return false;
                }
                return true;
            }
            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public IAsyncCommand SaveStepCommand { get; }
        public IAsyncCommand NavigateBackCommand { get; }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is AchievementStep model)
            {
                _model = model;
                Title = _model.Title;
                NotesText = _model.Description;
                TimeSpent = _model.TimeSpent.ToString();
            }
            return base.InitializeAsync(navigationData);
        }

        private async Task SaveStep()
        {
            if (double.TryParse(TimeSpent, out var result))
            {
                _model.TimeSpent = result;
            }
            _model.Title = Title;
            _model.Description = NotesText;
            _model.AchievedDate = DateTime.Now;

            App.AchievementStepRepository.Value.Save(_model);

            MessagingCenter.Send(this, ConstantsHelper.AchievementStepEditComplete);
            await NavigateBack();
        }

        private async Task NavigateBack()
        {
            await NavigationService.NavigatePopupBackAsync();
        }
    }
}