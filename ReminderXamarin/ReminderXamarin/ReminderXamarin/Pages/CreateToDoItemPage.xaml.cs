using System;
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
                await Navigation.PopAsync();
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