using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

using Acr.UserDialogs;
using PropertyChanged;
using ReminderXamarin.Core.Interfaces;
using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Services.FilePickerService;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

using Rm.Data.Data.Entities;
using Rm.Helpers;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.ViewModels
{
    [Preserve(AllMembers = true)]
    public class UserProfileViewModel : BaseNavigableViewModel
    {
        private readonly IFileSystem _fileService;
        private readonly IMediaService _mediaService;
        private readonly IPlatformDocumentPicker _documentPicker;

        public UserProfileViewModel(
            INavigationService navigationService,
            IFileSystem fileService,
            IMediaService mediaService,
            IPlatformDocumentPicker documentPicker,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            _fileService = fileService;
            _mediaService = mediaService;
            _documentPicker = documentPicker;

            ImageContent = new byte[0];

            ChangeUserProfileCommand = commandResolver.AsyncCommand(ChangeUserProfile);
            UpdateUserCommand = commandResolver.Command(UpdateUser);
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
        
        [AlsoNotifyFor(nameof(UserProfileImageSource))]
        public byte[] ImageContent { get; set; }

        public ImageSource UserProfileImageSource
        {
            get
            {
                if (ImageContent == null || ImageContent.Length == 0)
                {
                    return ImageSource.FromResource(
                        ConstantsHelper.NoPhotoImage);
                }

                return ImageSource.FromStream(
                    () => new MemoryStream(ImageContent));
            }
        }
        public int NotesCount { get; set; }
        public int AchievementsCount { get; set; }
        public int FriendBirthdaysCount { get; set; }
        public bool ViewModelChanged { get; set; }

        public ICommand ChangeUserProfileCommand { get; }
        public ICommand UpdateUserCommand { get; }

        private async Task ChangeUserProfile()
        {
            var document = await _documentPicker.DisplayImageImportAsync();
             
            if (document == null)
            {
                return;
            }
            
            // Ensure that user downloads .png or .jpg file as profile icon.
            if (document.Name.EndsWith(".png") 
                || document.Name.EndsWith(".jpg") 
                || document.Name.EndsWith(".jpeg"))
            {
                try
                {
                    ViewModelChanged = true;
                    await Task.Run(() =>
                    {
                        var imageContent = _fileService.ReadAllBytes(document.Path);
                        var resizedImage = _mediaService.ResizeImage(
                            imageContent,
                            ConstantsHelper.ResizedImageWidth,
                            ConstantsHelper.ResizedImageHeight);

                        ImageContent = resizedImage;
                    }).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync(ex.Message);
                }
            }
        }

        private void UpdateUser()
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