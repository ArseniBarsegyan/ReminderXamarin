using Rg.Plugins.Popup.Pages;

namespace ReminderXamarin.Views
{
    public partial class NewToDoView : PopupPage
    {
        public NewToDoView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DescriptionEntry.Focus();
        }
    }
}