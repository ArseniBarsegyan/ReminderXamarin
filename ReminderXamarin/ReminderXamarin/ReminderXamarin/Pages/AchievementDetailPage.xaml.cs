using System;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementDetailPage : ContentPage
    {
        private readonly AchievementViewModel _viewModel;

        public AchievementDetailPage(AchievementViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _viewModel = viewModel;
        }

        private void AchievementNotes_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
        }

        private async void AddNoteButton_OnClicked(object sender, EventArgs e)
        {
        }
    }
}