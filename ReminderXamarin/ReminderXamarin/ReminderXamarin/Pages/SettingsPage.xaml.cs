using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel.SaveSettingsCommand.Execute(null);
        }

        private void PinEntry_OnCompleted(object sender, EventArgs e)
        {
            ViewModel.Pin = PinEntry.Text;
        }
    }
}