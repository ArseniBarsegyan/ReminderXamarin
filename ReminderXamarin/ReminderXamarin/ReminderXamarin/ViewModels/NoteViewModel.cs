using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.Extensions;
using ReminderXamarin.Helpers;
using ReminderXamarin.Services;
using ReminderXamarin.Services.FilePickerService;
using ReminderXamarin.ViewModels.Base;
using ReminderXamarin.Data.Entities;
using Xamarin.Forms;
using IFileSystem = ReminderXamarin.Services.IFileSystem;

namespace ReminderXamarin.ViewModels
{
    public class NoteViewModel : BaseViewModel
    {
        private readonly MediaHelper _mediaHelper;
        private readonly TransformHelper _transformHelper;
        private static readonly IPermissionService PermissionService = DependencyService.Get<IPermissionService>();

        public NoteViewModel()
        {
            _mediaHelper = new MediaHelper();
            _transformHelper = new TransformHelper();
            Photos = new ObservableCollection<PhotoViewModel>();
            Videos = new ObservableCollection<VideoViewModel>();

            TakePhotoCommand = new Command(async () => await TakePhotoCommandExecute());
            DeletePhotoCommand = new Command<int>(DeletePhotoCommandExecute);
            TakeVideoCommand = new Command(async () => await TakeVideoCommandExecute());
            PickPhotoCommand = new Command<PlatformDocument>(async document => await PickPhotoCommandExecute(document));
            PickVideoCommand = new Command<PlatformDocument>(async document => await PickVideoCommandExecute(document));
            CreateNoteCommand = new Command(async() => await CreateNoteCommandExecute());
            UpdateNoteCommand = new Command(async() => await UpdateNoteCommandExecute());
            DeleteNoteCommand = new Command(async() => await DeleteNoteCommandExecute());
        }

        public PhotoViewModel SelectedPhoto { get; set; }
        public ObservableCollection<PhotoViewModel> Photos { get; set; }
        public ObservableCollection<VideoViewModel> Videos { get; set; }

        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EditDate { get; set; }
        public string FullDescription { get; set; }
        public bool IsLoading { get; set; }
        
        public ICommand DeletePhotoCommand { get; set; }
        public ICommand TakePhotoCommand { get; set; }
        public ICommand TakeVideoCommand { get; set; }
        public ICommand PickPhotoCommand { get; set; }
        public ICommand PickVideoCommand { get; set; }
        public ICommand CreateNoteCommand { get; set; }
        public ICommand UpdateNoteCommand { get; set; }
        public ICommand DeleteNoteCommand { get; set; }

        /// <summary>
        /// Invokes when Photos collection changing.
        /// </summary>
        public event EventHandler PhotosCollectionChanged;

        private async Task TakePhotoCommandExecute()
        {
            bool permissionResult = await PermissionService.AskPermission();
            if (permissionResult)
            {
                IsLoading = true;
                var photoModel = await _mediaHelper.TakePhotoAsync();
                if (photoModel != null)
                {
                    Photos.Add(photoModel.ToPhotoViewModel());
                    PhotosCollectionChanged?.Invoke(this, EventArgs.Empty);
                }
                IsLoading = false;
            }
        }

        private async Task PickPhotoCommandExecute(PlatformDocument document)
        {
            if (document.Name.EndsWith(".png") || document.Name.EndsWith(".jpg"))
            {
                var photoModel = new PhotoModel
                {
                    NoteId = Id
                };

                var mediaService = DependencyService.Get<IMediaService>();
                var fileSystem = DependencyService.Get<IFileSystem>();
                var imageContent = fileSystem.ReadAllBytes(document.Path);

                var resizedImage = mediaService.ResizeImage(imageContent, ConstantsHelper.ResizedImageWidth, ConstantsHelper.ResizedImageHeight);
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string imagePath = Path.Combine(path, document.Name);

                File.WriteAllBytes(imagePath, resizedImage);
                photoModel.ResizedPath = imagePath;
                photoModel.Thumbnail = imagePath;

                await _transformHelper.ResizeAsync(imagePath, photoModel);

                Photos.Add(photoModel.ToPhotoViewModel());
                PhotosCollectionChanged?.Invoke(this, EventArgs.Empty);
            }
            IsLoading = false;
        }

        private void DeletePhotoCommandExecute(int position)
        {
            IsLoading = true;
            if (Photos.Any())
            {
                Photos.RemoveAt(position);
                PhotosCollectionChanged?.Invoke(this, EventArgs.Empty);
            }
            IsLoading = false;
        }

        private async Task TakeVideoCommandExecute()
        {
            bool permissionResult = await PermissionService.AskPermission();

            if (permissionResult)
            {
                IsLoading = true;
                var videoModel = await _mediaHelper.TakeVideoAsync();
                if (videoModel != null)
                {
                    var photoModel = new PhotoModel
                    {
                        NoteId = Id,
                        IsVideo = true
                    };
                    var videoName =
                        videoModel.Path.Substring(videoModel.Path.LastIndexOf(@"/", StringComparison.InvariantCulture) + 1);
                    var imageName = videoName.Substring(0, videoName.Length - 4) + "_thumb.jpg";

                    var mediaService = DependencyService.Get<IMediaService>();
                    var imageContent = mediaService.GenerateThumbImage(videoModel.Path, ConstantsHelper.ThumbnailTimeFrame);

                    var resizedImage = mediaService.ResizeImage(imageContent, ConstantsHelper.ResizedImageWidth, ConstantsHelper.ResizedImageHeight);
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    string imagePath = Path.Combine(path, imageName);

                    File.WriteAllBytes(imagePath, resizedImage);
                    photoModel.ResizedPath = imagePath;
                    photoModel.Thumbnail = imagePath;

                    await _transformHelper.ResizeAsync(imagePath, photoModel);

                    Photos.Add(photoModel.ToPhotoViewModel());
                    PhotosCollectionChanged?.Invoke(this, EventArgs.Empty);

                    Videos.Add(videoModel.ToVideoViewModel());
                }
                IsLoading = false;
            }
        }

        private async Task PickVideoCommandExecute(PlatformDocument document)
        {
            if (document.Name.EndsWith(".mp4"))
            {
                var photoModel = new PhotoModel
                {
                    NoteId = Id,
                    IsVideo = true
                };
                
                var videoName =
                    document.Path.Substring(document.Path.LastIndexOf(@"/", StringComparison.InvariantCulture) + 1);
                var imageName = videoName.Substring(0, videoName.Length - 4) + "_thumb.jpg";

                var mediaService = DependencyService.Get<IMediaService>();
                var fileHelper = DependencyService.Get<IFileHelper>();
                var fileSystem = DependencyService.Get<IFileSystem>();

                // Thumbnail
                var imageContent = mediaService.GenerateThumbImage(document.Path, ConstantsHelper.ThumbnailTimeFrame);
                var resizedImage = mediaService.ResizeImage(imageContent, ConstantsHelper.ResizedImageWidth, ConstantsHelper.ResizedImageHeight);
                //string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                //string imagePath = Path.Combine(path, imageName);
                string imagePath = fileHelper.GetLocalFilePath(imageName);
                File.WriteAllBytes(imagePath, resizedImage);
                
                // Video
                var videoContent = fileSystem.ReadAllBytes(document.Path);
                string videoPath = fileHelper.GetVideoSavingPath(document.Name);
                if (string.IsNullOrEmpty(videoPath))
                {
                    videoPath = fileHelper.GetLocalFilePath(document.Path);
                }
                File.WriteAllBytes(videoPath, videoContent);

                var videoModel = new VideoModel
                {
                    NoteId = Id,
                    Path = videoPath
                };

                photoModel.ResizedPath = imagePath;
                photoModel.Thumbnail = imagePath;

                await _transformHelper.ResizeAsync(imagePath, photoModel);

                Photos.Add(photoModel.ToPhotoViewModel());
                PhotosCollectionChanged?.Invoke(this, EventArgs.Empty);

                Videos.Add(videoModel.ToVideoViewModel());
            }
            IsLoading = false;
        }

        private async Task CreateNoteCommandExecute()
        {
            App.NoteRepository.Save(this.ToNoteModel());
            IsLoading = false;
        }

        private async Task UpdateNoteCommandExecute()
        {
            IsLoading = true;
            // Update edit date since user pressed confirm
            EditDate = DateTime.Now;
            App.NoteRepository.Save(this.ToNoteModel());
            IsLoading = false;
        }

        private async Task DeleteNoteCommandExecute()
        {
            App.NoteRepository.DeleteNote(this.ToNoteModel());
        }
    }
}