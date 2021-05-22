using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.Views
{
    [Preserve(AllMembers = true)]
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