using System;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToDoPage : ContentPage
    {
        public ToDoPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }

        private void Delete_OnClicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var toDoViewModel = menuItem?.CommandParameter as ToDoViewModel;
            toDoViewModel?.DeleteItemCommand.Execute(toDoViewModel);
            ViewModel.OnAppearing();
        }

        private async void Create_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateToDoItemPage());
        }
    }
}