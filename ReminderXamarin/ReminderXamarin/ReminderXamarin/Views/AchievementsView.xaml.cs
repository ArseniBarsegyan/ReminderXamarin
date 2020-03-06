using ReminderXamarin.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementsView : ContentPage
    {
        public AchievementsView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is AchievementsViewModel vm)
            {
                vm.OnAppearing();
            }
        }
    }
}