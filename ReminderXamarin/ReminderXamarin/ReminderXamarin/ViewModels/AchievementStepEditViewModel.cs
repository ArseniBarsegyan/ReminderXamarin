using System;
using System.Threading.Tasks;

using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

using Rm.Helpers;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.ViewModels
{
    [Preserve(AllMembers = true)]
    public class AchievementStepEditViewModel : BaseNavigableViewModel
    {
        private bool _isEnabled;
        private AchievementStepViewModel _viewModel;

        public AchievementStepEditViewModel(
            INavigationService navigationService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            SaveStepCommand = commandResolver.AsyncCommand(SaveStep, () => IsEnabled);
            NavigateBackCommand = commandResolver.AsyncCommand(NavigateBack);
        }
                
        public string Title { get; set; }
        public string NotesText { get; set; }
        public string TimeSpent { get; set; }
        public DateTime AchievedDate { get; set; }
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
            if (navigationData is AchievementStepViewModel model)
            {
                _viewModel = model;
                Title = _viewModel.Title;
                NotesText = _viewModel.Description;
                TimeSpent = _viewModel.TimeSpent.ToString();
                AchievedDate =
                    _viewModel.AchievedDate == DateTime.MinValue 
                    ? DateTime.Now 
                    : _viewModel.AchievedDate;
            }
            return base.InitializeAsync(navigationData);
        }

        private async Task SaveStep()
        {
            if (double.TryParse(TimeSpent, out var result))
            {
                _viewModel.TimeSpent = result;
            }
            _viewModel.Title = Title;
            _viewModel.Description = NotesText;
            _viewModel.AchievedDate = AchievedDate;

            // App.AchievementStepRepository.Value.Save(_model);

            MessagingCenter.Send(this, ConstantsHelper.AchievementStepEditComplete, _viewModel);
            await NavigateBack();
        }

        private async Task NavigateBack()
        {
            await NavigationService.NavigatePopupBackAsync();
        }
    }
}