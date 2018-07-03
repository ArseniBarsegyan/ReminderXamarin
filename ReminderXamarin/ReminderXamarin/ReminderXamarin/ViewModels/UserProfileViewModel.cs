using System.Windows.Input;
using ReminderXamarin.Interfaces;
using ReminderXamarin.Interfaces.FilePickerService;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class UserProfileViewModel : BaseViewModel
    {
        private static readonly IFileSystem FileSystemService = DependencyService.Get<IFileSystem>();

        public UserProfileViewModel()
        {
            ChangeUserProfileCommand = new Command<PlatformDocument>(ChangeUserProfileCommandExecute);
            UserName = "Arseni";
            ImageContent = new byte[0];
        }

        public string UserName { get; set; }
        public byte[] ImageContent { get; set; }

        public ICommand ChangeUserProfileCommand { get; set; }

        private void ChangeUserProfileCommandExecute(PlatformDocument document)
        {
            // Ensure that user downloads .png or .jpg file as profile icon.
            if (document.Name.EndsWith(".png") || document.Name.EndsWith(".jpg"))
            {
                ImageContent = FileSystemService.ReadAllBytes(document.Path);
            }
        }
    }
}