using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementDetailsView : ContentPage
    {
        public AchievementDetailsView()
        {
            InitializeComponent();
        }

        private async void AddAchievementStepButton_OnClicked(object sender, EventArgs e)
        {
        }
    }
}