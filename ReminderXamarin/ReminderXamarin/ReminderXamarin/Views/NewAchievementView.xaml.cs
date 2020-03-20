using Rg.Plugins.Popup.Pages;

using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
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