using System;

using ReminderXamarin.DependencyResolver;
using ReminderXamarin.Services.FilePickerService;
using ReminderXamarin.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfileView : ContentPage
    {
        private static readonly IPlatformDocumentPicker DocumentPicker = ComponentFactory.Resolve<IPlatformDocumentPicker>();

        public UserProfileView()
        {
            InitializeComponent();
        }

        private async void PickUserPhotoImage_OnClicked(object sender, EventArgs e)
        {
            var document = await DocumentPicker.DisplayImportAsync(this);
            if (document == null)
            {
                return;
            }
            if (BindingContext is UserProfileViewModel viewModel)
            {
                viewModel.ChangeUserProfileCommand.Execute(document);
            }
        }
    }
}