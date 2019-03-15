using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Helpers;
using ReminderXamarin.Services;
using ReminderXamarin.Services.FilePickerService;
using Xamarin.Forms;
using ReminderXamarin.ViewModels.Base;
using RI.Data.Data.Entities;

namespace ReminderXamarin.ViewModels
{
    public class UserProfileViewModel : BaseViewModel
    {
        private static readonly IFileSystem FileService = DependencyService.Get<IFileSystem>();
        private static readonly IMediaService MediaService = DependencyService.Get<IMediaService>();

        public UserProfileViewModel()
        {
            ImageContent = new byte[0];

            ChangeUserProfileCommand = new Command<PlatformDocument>(ChangeUserProfileCommandExecute);
            UpdateUserCommand = new Command(async () => await UpdateUserCommandExecute());
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is AppUser appUser)
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
        
        private void ChangeUserProfileCommandExecute(PlatformDocument document)
        {
            // Ensure that user downloads .png or .jpg file as profile icon.
            if (document.Name.EndsWith(".png") || document.Name.EndsWith(".jpg"))
            {
                ViewModelChanged = true;
                var imageContent = FileService.ReadAllBytes(document.Path);
                var resizedImage = MediaService.ResizeImage(imageContent, ConstantsHelper.ResizedImageWidth, ConstantsHelper.ResizedImageHeight);

                ImageContent = resizedImage;
            }
        }

        private async Task UpdateUserCommandExecute()
        {
            var user = App.UserRepository.GetOne(x => x.Id == Id.ToString());
            if (user != null)
            {
                App.UserRepository.RealmInstance.Write(() =>
                {
                    user.ImageContent = ImageContent;
                    user.UserName = UserName;
                });
                MessagingCenter.Send(this, ConstantsHelper.ProfileUpdated);
                ViewModelChanged = false;
            }
        }
    }
}