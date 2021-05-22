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
        private bool _isFirstOpening = true;
        private ToDoCalendarViewModel ViewModel => BindingContext as ToDoCalendarViewModel;

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
                await Task.Delay(500);
                MonthCollectionView.ScrollTo(ViewModel.Months.ElementAt(1),animate:false);
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
    }
}