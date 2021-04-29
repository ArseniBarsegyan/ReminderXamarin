using ReminderXamarin.ViewModels;

using Xamarin.Forms;

namespace ReminderXamarin.Views
{
    public partial class SettingsView : ContentPage
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (BindingContext is SettingsViewModel viewModel)
            {
                viewModel.OnDisappearing();
            }
        }
    }
}