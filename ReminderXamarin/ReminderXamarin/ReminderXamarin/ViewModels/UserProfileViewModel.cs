using System.Windows.Input;
using ReminderXamarin.Extensions;
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
            ImageContent = new byte[0];

            ChangeUserProfileCommand = new Command<PlatformDocument>(ChangeUserProfileCommandExecute);
            UpdateUserCommand = new Command(UpdateUserCommandExecute);
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public byte[] ImageContent { get; set; }

        public ICommand ChangeUserProfileCommand { get; set; }
        public ICommand UpdateUserCommand { get; set; }

        public void OnAppearing()
        {
        }

        private void ChangeUserProfileCommandExecute(PlatformDocument document)
        {
            // Ensure that user downloads .png or .jpg file as profile icon.
            if (document.Name.EndsWith(".png") || document.Name.EndsWith(".jpg"))
            {
                ImageContent = FileSystemService.ReadAllBytes(document.Path);
            }
        }

        private void UpdateUserCommandExecute()
        {
            App.UserRepository.Save(this.ToUserModel());
        }
    }
}