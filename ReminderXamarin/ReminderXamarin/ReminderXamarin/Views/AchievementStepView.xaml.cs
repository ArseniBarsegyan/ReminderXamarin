using System;

using ReminderXamarin.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementStepView : ContentPage
    {
        public AchievementStepView()
        {
            InitializeComponent();
        }

        private void OnViewChanged(object sender, EventArgs e)
        {
            if (BindingContext is AchievementStepViewModel viewModel)
            {
                viewModel.ViewModelChanged = true;
            }
        }
    }
}