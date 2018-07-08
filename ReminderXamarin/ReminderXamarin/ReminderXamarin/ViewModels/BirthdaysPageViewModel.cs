using ReminderXamarin.Extensions;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class BirthdaysPageViewModel : BaseViewModel
    {
        public BirthdaysPageViewModel()
        {
            FriendViewModels = new ObservableCollection<FriendViewModel>();
            RefreshListCommand = new Command(RefreshCommandExecute);
        }

        public bool IsRefreshing { get; set; }
        public ObservableCollection<FriendViewModel> FriendViewModels { get; set; }

        public ICommand RefreshListCommand { get; set; }

        public void OnAppearing()
        {
            LoadFriendsFromDatabase();
        }

        private void RefreshCommandExecute()
        {
            IsRefreshing = true;
            LoadFriendsFromDatabase();
            IsRefreshing = false;
        }

        private void LoadFriendsFromDatabase()
        {
            FriendViewModels = App.FriendsRepository
                .GetAll()
                .ToFriendViewModels()
                .OrderByDescending(x => x.BirthDayDate)
                .ToObservableCollection();
        }
    }
}