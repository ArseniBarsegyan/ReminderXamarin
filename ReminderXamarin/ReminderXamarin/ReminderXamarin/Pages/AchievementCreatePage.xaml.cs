using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementCreatePage : ContentPage
    {
        public AchievementCreatePage()
        {
            InitializeComponent();
        }

        private async void Save_OnClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleEditor.Text))
            {
                await Navigation.PopAsync();
                return;
            }
            ViewModel.Title = TitleEditor.Text;
            ViewModel.GeneralDescription = DescriptionEditor.Text;
            ViewModel.CreateAchievementCommand.Execute(null);
            await Navigation.PopAsync();
        }
    }
}