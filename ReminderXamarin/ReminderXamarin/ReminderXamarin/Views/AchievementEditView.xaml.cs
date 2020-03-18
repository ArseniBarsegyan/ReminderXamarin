using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementEditView : ContentPage
    {
        public AchievementEditView()
        {
            InitializeComponent();
        }       

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            AchievementStepsListView.SelectedItem = null;
        }
    }
}