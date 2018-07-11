using System.Linq;
using System.Windows.Input;
using ReminderXamarin.Extensions;
using ReminderXamarin.Interfaces;
using ReminderXamarin.Interfaces.FilePickerService;
using ReminderXamarin.Models;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class UserProfileViewModel : BaseViewModel
    {
        private static readonly IFileSystem FileSystemService = DependencyService.Get<IFileSystem>();
        private readonly UserModel _userModel;

        public UserProfileViewModel()
        {
            ImageContent = new byte[0];

            ChangeUserProfileCommand = new Command<PlatformDocument>(ChangeUserProfileCommandExecute);
            UpdateUserCommand = new Command(UpdateUserCommandExecute);

            var user = App.UserRepository.GetAll().LastOrDefault(x => x.UserName == "Arseni");
            if (user != null)
            {
                _userModel = user;
            }
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] ImageContent { get; set; }

        public ICommand ChangeUserProfileCommand { get; set; }
        public ICommand UpdateUserCommand { get; set; }

        public void OnAppearing()
        {
            if (_userModel != null)
            {
                Id = _userModel.Id;
                UserName = _userModel.UserName;
                ImageContent = _userModel.ImageContent;
            }
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