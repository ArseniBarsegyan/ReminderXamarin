using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.Views
{
    [Preserve(AllMembers = true)]
    public partial class NewAchievementView : PopupPage
    {
        public NewAchievementView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            TitleEntry.Focus();
        }
    }
}