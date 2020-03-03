using System;

using ReminderXamarin.DependencyResolver;
using ReminderXamarin.Services.FilePickerService;
using ReminderXamarin.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementStepView : ContentPage
    {
        private static readonly IPlatformDocumentPicker DocumentPicker = ComponentFactory.Resolve<IPlatformDocumentPicker>();

        public AchievementStepView()
        {
            InitializeComponent();
        }

        private async void AchievementStepImage_OnTapped(object sender, EventArgs e)
        {
            var document = await DocumentPicker.DisplayImportAsync(this);
            if (document == null)
            {
                return;
            }
            if (BindingContext is AchievementStepViewModel viewModel)
            {
                viewModel.ChangeImageCommand.Execute(document);
            }
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