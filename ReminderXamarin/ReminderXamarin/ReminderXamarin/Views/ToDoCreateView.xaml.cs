using System;

using ReminderXamarin.Enums;
using ReminderXamarin.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToDoCreateView : ContentPage
    {
        private readonly ToDoStatus _toDoStatus;

        public ToDoCreateView(ToDoStatus toDoStatus)
        {
            _toDoStatus = toDoStatus;
            InitializeComponent();
            TimePicker.Time = DateTime.Now.TimeOfDay;
        }

        private async void Save_OnClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DescriptionEditor.Text))
            {
                await Navigation.PopAsync();
                return;
            }

            var eventDate = DatePicker.Date;
            var eventTime = TimePicker.Time;

            var fullDate = eventDate.Add(eventTime);

            if (BindingContext is ToDoViewModel viewModel)
            {
                viewModel.Status = _toDoStatus;
                viewModel.WhenHappens = fullDate;
                viewModel.Description = DescriptionEditor.Text;
                viewModel.CreateToDoCommand.Execute(null);
            }            

            await Navigation.PopAsync();
        }
    }
}