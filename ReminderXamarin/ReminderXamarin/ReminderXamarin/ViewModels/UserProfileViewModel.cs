using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Rm.Helpers;
using ReminderXamarin.Services;
using ReminderXamarin.Services.FilePickerService;
using Rm.Data.Data.Entities;
using Xamarin.Forms;
using ReminderXamarin.ViewModels.Base;

namespace ReminderXamarin.ViewModels
{
    public class UserProfileViewModel : BaseViewModel
    {
        private static readonly IFileSystem FileService = DependencyService.Get<IFileSystem>();
        private static readonly IMediaService MediaService = DependencyService.Get<IMediaService>();

        public UserProfileViewModel()
        {
            ImageContent = new byte[0];

            ChangeUserProfileCommand = new Command<PlatformDocument>(ChangeUserProfile);
            UpdateUserCommand = new Command(async () => await UpdateUser());
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is UserModel appUser)
            {
                Id = appUser.Id;
                UserName = appUser.UserName;
                ImageContent = appUser.ImageContent;
                NotesCount = appUser.Notes.Count;
                AchievementsCount = appUser.Achievements.Count;
                FriendBirthdaysCount = appUser.Birthdays.Count;
            }
            return base.InitializeAsync(navigationData);
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public byte[] ImageContent { get; set; }
        public int NotesCount { get; set; }
        public int AchievementsCount { get; set; }
        public int FriendBirthdaysCount { get; set; }
        public bool ViewModelChanged { get; set; }

        public ICommand ChangeUserProfileCommand { get; set; }
        public ICommand UpdateUserCommand { get; set; }
        
        private async void ChangeUserProfile(PlatformDocument document)
        {
            // Ensure that user downloads .png or .jpg file as profile icon.
            if (document.Name.EndsWith(".png") || document.Name.EndsWith(".jpg"))
            {
                try
                {
                    ViewModelChanged = true;
                    var imageContent = FileService.ReadAllBytes(document.Path);
                    var resizedImage = MediaService.ResizeImage(imageContent, ConstantsHelper.ResizedImageWidth,
                        ConstantsHelper.ResizedImageHeight);

                    ImageContent = resizedImage;
                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync(ex.Message);
                }
            }
        }

        private async Task UpdateUser()
        {
            var user = App.UserRepository.GetUserAsync(Id);
            if (user != null)
            {
                user.ImageContent = ImageContent;
                user.UserName = UserName;
                App.UserRepository.Save(user);
                MessagingCenter.Send(this, ConstantsHelper.ProfileUpdated);
                ViewModelChanged = false;
            }
        }
    }
}