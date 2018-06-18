using System;
using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateToDoItemPage : ContentPage
    {
        public CreateToDoItemPage()
        {
            InitializeComponent();
        }

        private async void Save_OnClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DescriptionEditor.Text))
            {
                await DisplayAlert(ConstantHelper.Warning, ConstantHelper.ToDoTextIsEmptyMessage, ConstantHelper.Ok);
                return;
            }

            DateTime currentDateTime = DateTime.Now;

            ViewModel.CreateToDoItemCommand.Execute(new ToDoViewModel
            {
                CreationDate = currentDateTime,
                EditDate = currentDateTime,
                Description = DescriptionEditor.Text,
            });

            await Navigation.PopAsync();
        }
    }
}