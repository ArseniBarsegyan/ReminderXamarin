using System;

using ReminderXamarin.DependencyResolver;
using ReminderXamarin.Services.FilePickerService;
using ReminderXamarin.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.Views
{
    [Preserve(AllMembers = true)]
    public partial class UserProfileView : ContentPage
    {
        private readonly IPlatformDocumentPicker _documentPicker;
        private UserProfileViewModel ViewModel => BindingContext as UserProfileViewModel;

        public UserProfileView()
        {
            InitializeComponent();
            _documentPicker =  ComponentFactory.Resolve<IPlatformDocumentPicker>();
        }

        private async void PickUserPhotoImageOnClicked(object sender, EventArgs e)
        {
            var document = await _documentPicker.DisplayImportAsync(this);
            if (document == null)
            {
                return;
            }
            
            ViewModel.ChangeUserProfileCommand.Execute(document);
        }
    }
}