using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Extensions;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

using Rm.Helpers;

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReminderXamarin.ViewModels
{
    public class BirthdaysViewModel : BaseViewModel
    {
        private ICommandResolver _commandResolver;

        public BirthdaysViewModel(INavigationService navigationService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            _commandResolver = commandResolver;

            BirthdayViewModels = new ObservableCollection<BirthdayViewModel>();
            RefreshListCommand = commandResolver.Command(Refresh);
            NavigateToEditBirthdayCommand = commandResolver.AsyncCommand<int>(NavigateToEditView);
        }

        public bool IsRefreshing { get; set; }
        public ObservableCollection<BirthdayViewModel> BirthdayViewModels { get; set; }

        public ICommand NavigateToEditBirthdayCommand { get; }

        public ICommand RefreshListCommand { get; }

        public void OnAppearing()
        {
            LoadFriendsFromDatabase();
        }

        private void Refresh()
        {
            IsRefreshing = true;
            LoadFriendsFromDatabase();
            IsRefreshing = false;
        }

        private void LoadFriendsFromDatabase()
        {
            BirthdayViewModels = App.BirthdaysRepository.Value
                .GetAll(x => x.UserId == Settings.CurrentUserId)
                .ToFriendViewModels(NavigationService, _commandResolver)
                .OrderByDescending(x => x.BirthDayDate)
                .ToObservableCollection();
        }

        private async Task NavigateToEditView(int id)
        {
            await NavigationService.NavigateToAsync<BirthdayEditViewModel>(id).ConfigureAwait(false);
        }
    }
}