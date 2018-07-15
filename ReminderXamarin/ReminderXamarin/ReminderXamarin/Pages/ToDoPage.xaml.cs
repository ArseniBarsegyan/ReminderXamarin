using System;
using ReminderXamarin.Helpers;
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

        private async void Delete_OnClicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert
                (ConstantHelper.Warning, ConstantHelper.ToDoItemDeleteMessage, ConstantHelper.Ok, ConstantHelper.Cancel);
            if (result)
            {
                var menuItem = sender as MenuItem;
                var toDoViewModel = menuItem?.CommandParameter as ToDoViewModel;
                toDoViewModel?.DeleteItemCommand.Execute(toDoViewModel);
                ViewModel.OnAppearing();
            }
        }        

        private async void ToDoList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is ToDoViewModel viewModel)
            {
                await Navigation.PushAsync(new ToDoDetailPage(viewModel));
            }
            ToDoList.SelectedItem = null;
        }

        private async void CreateToDoButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ToDoCreatePage());
        }
    }
}