using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ReminderXamarin.Collections;
using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Core.Interfaces.Commanding.AsyncCommanding;
using ReminderXamarin.Extensions;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;
using Rm.Data.Data.Repositories;
using Rm.Helpers;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.ViewModels
{
    [Preserve(AllMembers = true)]
    public class BirthdaysViewModel : BaseNavigableViewModel
    {
        private BirthdaysRepository BirthdaysRepository => App.BirthdaysRepository?.Value;
        
        public BirthdaysViewModel(
            INavigationService navigationService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            Birthdays = new RangeObservableCollection<BirthdayViewModel>();
            
            RefreshCommand = commandResolver.Command(Refresh);
            NavigateToEditBirthdayCommand = commandResolver.AsyncCommand<int>(EditBirthday);
            DeleteBirthdayCommand = commandResolver.AsyncCommand<BirthdayViewModel>(DeleteBirthday);
        }
        
        public bool IsRefreshing { get; private set; }
        
        public RangeObservableCollection<BirthdayViewModel> Birthdays { get; }
        public ICommand RefreshCommand { get; }
        public ICommand DeleteBirthdayCommand { get; }
        public IAsyncCommand<int> NavigateToEditBirthdayCommand { get; }

        public void OnAppearing()
        {
            Refresh();
        }

        private void Refresh()
        {
            IsRefreshing = true;
            Birthdays.ReplaceRangeWithoutUpdating(BirthdaysRepository.GetAll().ToAchievementStepViewModels());
            Birthdays.RaiseCollectionChanged();
            IsRefreshing = false;
        }
        
        private async Task EditBirthday(int id)
        {
            await NavigationService.NavigateToAsync<BirthdayEditViewModel>(id);
        }

        private async Task DeleteBirthday(BirthdayViewModel viewModel)
        {
            bool result = await UserDialogs.Instance.ConfirmAsync(
                ConstantsHelper.BirthdaysDeleteMessage,
                ConstantsHelper.Warning,
                ConstantsHelper.Ok,
                ConstantsHelper.Cancel);

            if (result)
            {
                Birthdays.Remove(viewModel);
                var modelToDelete = BirthdaysRepository.GetBirthdayAsync(viewModel.Id);
                if (modelToDelete != null)
                {
                    BirthdaysRepository.DeleteBirthday(modelToDelete);
                }
            }
        }
    }
}