using Acr.UserDialogs;

using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Services;
using ReminderXamarin.Services.FilePickerService;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

using Rm.Data.Data.Entities;
using Rm.Helpers;

using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class UserProfileViewModel : BaseViewModel
    {
        private readonly IFileSystem _fileService;
        private readonly IMediaService _mediaService;

        public UserProfileViewModel(INavigationService navigationService,
            IFileSystem fileService,
            IMediaService mediaService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            _fileService = fileService;
            _mediaService = mediaService;

            ImageContent = new byte[0];

            ChangeUserProfileCommand = commandResolver.Command<PlatformDocument>(ChangeUserProfile);
            UpdateUserCommand = commandResolver.AsyncCommand(UpdateUser);
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
            UpdateProfilePhoto();
            return base.InitializeAsync(navigationData);
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public byte[] ImageContent { get; set; }
        public ImageSource UserProfileImageSource { get; private set; }
        public int NotesCount { get; set; }
        public int AchievementsCount { get; set; }
        public int FriendBirthdaysCount { get; set; }
        public bool ViewModelChanged { get; set; }

        public ICommand ChangeUserProfileCommand { get; }
        public IAsyncCommand UpdateUserCommand { get; }

        private void UpdateProfilePhoto()
        {
            if (ImageContent == null || ImageContent.Length == 0)
            {
                UserProfileImageSource = ImageSource.FromResource(
                    ConstantsHelper.NoPhotoImage);
            }
            else
            {
                UserProfileImageSource = ImageSource.FromStream(
                    () => new MemoryStream(ImageContent));
            }
        }
        
        private async void ChangeUserProfile(PlatformDocument document)
        {
            // Ensure that user downloads .png or .jpg file as profile icon.
            if (document.Name.EndsWith(".png") || document.Name.EndsWith(".jpg") || document.Name.EndsWith(".jpeg"))
            {
                try
                {
                    ViewModelChanged = true;
                    var imageContent = _fileService.ReadAllBytes(document.Path);
                    var resizedImage = _mediaService.ResizeImage(imageContent, ConstantsHelper.ResizedImageWidth,
                        ConstantsHelper.ResizedImageHeight);

                    ImageContent = resizedImage;
                    UpdateProfilePhoto();
                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync(ex.Message);
                }
            }
        }

        private async Task UpdateUser()
        {
            var user = App.UserRepository.Value.GetUserAsync(Id);
            if (user != null)
            {
                user.ImageContent = ImageContent;
                user.UserName = UserName;
                App.UserRepository.Value.Save(user);
                MessagingCenter.Send(this, ConstantsHelper.ProfileUpdated);
                ViewModelChanged = false;
            }
        }
    }
}