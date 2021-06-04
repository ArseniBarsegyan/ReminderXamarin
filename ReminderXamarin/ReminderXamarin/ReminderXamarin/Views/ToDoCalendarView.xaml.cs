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
        private int _previousVisibleItemIndex;
        private bool _shouldSelectFirstDayWhenSwitchMonth = true;

        public ToDoCalendarView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel.OnDisappearing();
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

                ViewModel.CalendarViewModel.LoadDataIfNecessary(e.FirstVisibleItemIndex, _shouldSelectFirstDayWhenSwitchMonth);
                _previousVisibleItemIndex = e.FirstVisibleItemIndex;
            }
        }

        private void ToDoCollectionOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToDoCollection.SelectedItem = null;
            if (e.CurrentSelection.FirstOrDefault() is ToDoViewModel model)
            {
                ViewModel.ChangeToDoStatusCommand.Execute(model);
            }
        }

        private async void DatePickerOnDateSelected(object sender, DateChangedEventArgs e)
        {
            var month = ViewModel.GetMonthByDate(e.NewDate);
            if (month != null)
            {
                MonthCollectionView.ScrollTo(ViewModel.Months.IndexOf(month),animate:false);
            }

            _shouldSelectFirstDayWhenSwitchMonth = false;
            ViewModel.SelectDayCommand.Execute(e.NewDate);

            // Necessary in order not to fire ToDoCollectionOnSelectionChanged with false param
            await Task.Delay(800);
            _shouldSelectFirstDayWhenSwitchMonth = true;
        }
    }
}