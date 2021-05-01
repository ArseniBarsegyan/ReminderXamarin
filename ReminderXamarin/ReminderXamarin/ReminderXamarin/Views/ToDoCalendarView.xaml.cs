using ReminderXamarin.ViewModels;

using Xamarin.Forms;

namespace ReminderXamarin.Views
{
    public partial class ToDoCalendarView : ContentPage
    {
        public ToDoCalendarView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is ToDoCalendarViewModel vm)
            {
                vm.OnAppearing();
            }
            ReminderCalendarView.Subscribe();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (BindingContext is ToDoCalendarViewModel vm)
            {
                vm.OnDisappearing();
            }
            ReminderCalendarView.Unsubscribe();
        }

        private void ListViewOnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ToDoList.SelectedItem = null;
            if (e.SelectedItem is ToDoViewModel model)
            {
                if (BindingContext is ToDoCalendarViewModel viewModel)
                {
                    viewModel.ChangeToDoStatusCommand.Execute(model);
                }
            }
        }
    }
}