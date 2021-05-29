using System;
using System.Linq;
using System.Threading.Tasks;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.Views
{
    [Preserve(AllMembers = true)]
    public partial class ToDoCalendarView : ContentPage
    {
        private ToDoCalendarViewModel ViewModel => BindingContext as ToDoCalendarViewModel;
        private bool _isFirstOpening = true;
        private int _previousVisibleItemIndex;

        public ToDoCalendarView()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();

            if (_isFirstOpening)
            {
                const int timeUntilCollectionViewFullyLoad = 200;
                await Task.Delay(timeUntilCollectionViewFullyLoad);
                //MonthCollectionView.ScrollTo(ViewModel.Months.ElementAt(ViewModel.CurrentMonthIndex),animate:false);
                _isFirstOpening = false;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel.OnDisappearing();
        }

        private void ListViewOnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ToDoList.SelectedItem = null;
            if (e.SelectedItem is ToDoViewModel model)
            {
                ViewModel.ChangeToDoStatusCommand.Execute(model);
            }
        }

        private void MenuItemOnClicked(object sender, EventArgs e)
        {
            MonthCollectionView.ScrollTo(ViewModel.Months.ElementAt(ViewModel.CurrentMonthIndex),animate:false);
        }

        private void MonthCollectionViewOnScrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            if (e.FirstVisibleItemIndex == e.LastVisibleItemIndex)
            {
                if (_previousVisibleItemIndex == e.LastVisibleItemIndex)
                {
                    return;
                }

                ViewModel.LoadDataIfNecessary(e.FirstVisibleItemIndex);
                _previousVisibleItemIndex = e.FirstVisibleItemIndex;
            }
        }
    }
}