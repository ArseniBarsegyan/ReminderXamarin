using System;
using System.Threading.Tasks;
using System.Windows.Input;

using Acr.UserDialogs;

using ReminderXamarin.Services;
using ReminderXamarin.Services.FilePickerService;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

using Rm.Data.Data.Entities;
using Rm.Helpers;

using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class UserProfileViewModel : BaseViewModel
    {
        private readonly IFileSystem _fileService;
        private readonly IMediaService _mediaService;

        public UserProfileViewModel(INavigationService navigationService,
            IFileSystem fileService,
            IMediaService mediaService)
            : base(navigationService)
        {
            _fileService = fileService;
            _mediaService = mediaService;

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

        public ICommand ChangeUserProfileCommand { get; }
        public ICommand UpdateUserCommand { get; }
        
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