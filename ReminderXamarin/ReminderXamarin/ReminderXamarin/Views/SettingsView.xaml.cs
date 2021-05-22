using ReminderXamarin.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.Views
{
    [Preserve(AllMembers = true)]
    public partial class SettingsView : ContentPage
    {
        private SettingsViewModel ViewModel => BindingContext as SettingsViewModel;
        
        public SettingsView()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel.OnDisappearing();
        }
    }
}