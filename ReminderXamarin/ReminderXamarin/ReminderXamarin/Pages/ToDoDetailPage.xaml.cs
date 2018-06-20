using System;
using ReminderXamarin.Helpers;
using ReminderXamarin.Models;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToDoDetailPage : ContentPage
    {
        private readonly ToDoViewModel _viewModel;

        public ToDoDetailPage(ToDoViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _viewModel = viewModel;
            Title = $"{viewModel.EditDate:d}";
            DescriptionEditor.Text = viewModel.Description;
        }

        private async void Delete_OnClicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert
                (ConstantHelper.Warning, ConstantHelper.ToDoItemDeleteMessage, ConstantHelper.Ok, ConstantHelper.Cancel);
            if (result)
            {
                _viewModel.DeleteItemCommand.Execute(_viewModel);
                await Navigation.PopAsync();
            }
        }

        private void Confirm_OnClicked(object sender, EventArgs e)
        {
            _viewModel.Description = DescriptionEditor.Text;
            _viewModel.Priority = (ToDoPriority)PriorityPicker.SelectedItem;
            _viewModel.UpdateItemCommand.Execute(_viewModel);
        }
    }
}