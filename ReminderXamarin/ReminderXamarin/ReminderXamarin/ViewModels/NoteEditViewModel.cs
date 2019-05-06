using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using ReminderXamarin.Extensions;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ReminderXamarin.Services;
using ReminderXamarin.Services.FilePickerService;
using ReminderXamarin.ViewModels.Base;
using Rm.Data.Data.Entities;
using Rm.Helpers;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class NoteEditViewModel : BaseViewModel
    {
        private int _noteId;
        private Note _note;
        private readonly MediaHelper _mediaHelper;
        private readonly TransformHelper _transformHelper;
        private static readonly IPermissionService PermissionService = DependencyService.Get<IPermissionService>();
        
        public NoteEditViewModel()
        {
            _mediaHelper = new MediaHelper();
            _transformHelper = new TransformHelper();
            
            Photos = new ObservableCollection<PhotoViewModel>();
            Videos = new ObservableCollection<VideoViewModel>();

            TakePhotoCommand = new Command(async () => await TakePhoto());
            DeletePhotoCommand = new Command<int>(DeletePhoto);
            TakeVideoCommand = new Command(async () => await TakeVideo());
            PickPhotoCommand = new Command<PlatformDocument>(async document => await PickPhoto(document));
            PickVideoCommand = new Command<PlatformDocument>(async document => await PickVideo(document));
            SaveNoteCommand = new Command(async() => await SaveNote());
            DeleteNoteCommand = new Command(async () => await DeleteNote());
        }

        public override Task InitializeAsync(object navigationData)
        {
            _noteId = (int) navigationData;

            if (_noteId == 0)
            {
                Title = Resmgr.Value.GetString(ConstantsHelper.CreateNote, CultureInfo.CurrentCulture);
            }
            else
            {
                IsEditMode = true;
                _note = App.NoteRepository.GetNoteAsync(_noteId);
                Title = _note.EditDate.ToString("d");
                Description = _note.Description;
                Photos = _note.Photos.ToPhotoViewModels();
                Videos = _note.Videos.ToVideoViewModels();
                PhotosCollectionChanged?.Invoke(this, EventArgs.Empty);
            }
            return base.InitializeAsync(navigationData);
        }

        public bool IsEditMode { get; set; }
        public string Title { get; set; }
        public bool IsLoading { get; set; }
        public string Description { get; set; }
        public ObservableCollection<PhotoViewModel> Photos { get; set; }
        public ObservableCollection<VideoViewModel> Videos { get; set; }
        
        public ICommand DeletePhotoCommand { get; set; }
        public ICommand TakePhotoCommand { get; set; }
        public ICommand TakeVideoCommand { get; set; }
        public ICommand PickPhotoCommand { get; set; }
        public ICommand PickVideoCommand { get; set; }
        public ICommand SaveNoteCommand { get; set; }
        public ICommand DeleteNoteCommand { get; set; }

        /// <summary>
        /// Invokes when Photos collection changing.
        /// </summary>
        public event EventHandler PhotosCollectionChanged;

        private async Task TakePhoto()
        {
            bool permissionResult = await PermissionService.AskPermission();
            if (permissionResult)
            {
                IsLoading = true;
                try
                {
                    var photoModel = await _mediaHelper.TakePhotoAsync();
                    if (photoModel != null)
                    {
                        Photos.Add(photoModel.ToPhotoViewModel());
                        PhotosCollectionChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync(ex.Message);
                }
            }
            IsLoading = false;
        }

        private async Task PickPhoto(PlatformDocument document)
        {
            if (document.Name.EndsWith(".png") || document.Name.EndsWith(".jpg"))
            {
                IsLoading = true;
                try
                {
                    var photoModel = new PhotoModel
                    {
                        NoteId = _noteId
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
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync(ex.Message);
                }
            }
            IsLoading = false;
        }

        private void DeletePhoto(int position)
        {
            IsLoading = true;
            if (Photos.Any())
            {
                Photos.RemoveAt(position);
                PhotosCollectionChanged?.Invoke(this, EventArgs.Empty);
            }
            IsLoading = false;
        }

        private async Task TakeVideo()
        {
            bool permissionResult = await PermissionService.AskPermission();

            if (permissionResult)
            {
                IsLoading = true;
                var videoModel = await _mediaHelper.TakeVideoAsync();
                if (videoModel != null)
                {
                    try
                    {
                        var photoModel = new PhotoModel
                        {
                            NoteId = _noteId,
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
                    catch (Exception ex)
                    {
                        await UserDialogs.Instance.AlertAsync(ex.Message);
                    }
                }
                IsLoading = false;
            }
        }

        private async Task PickVideo(PlatformDocument document)
        {
            if (document.Name.EndsWith(".mp4"))
            {
                IsLoading = true;
                try
                {
                    var photoModel = new PhotoModel
                    {
                        NoteId = _noteId,
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
                        NoteId = _noteId,
                        Path = videoPath
                    };

                    photoModel.ResizedPath = imagePath;
                    photoModel.Thumbnail = imagePath;

                    await _transformHelper.ResizeAsync(imagePath, photoModel);

                    Photos.Add(photoModel.ToPhotoViewModel());
                    PhotosCollectionChanged?.Invoke(this, EventArgs.Empty);

                    Videos.Add(videoModel.ToVideoViewModel());
                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync(ex.Message);
                }
            }
            IsLoading = false;
        }

        private async Task SaveNote()
        {
            IsLoading = true;
            if (_noteId == 0)
            {
                var note = new Note
                {
                    CreationDate = DateTime.Now,
                    EditDate = DateTime.Now,
                    Description = Description,
                    Photos = Photos.ToPhotoModels().ToList(),
                    Videos = Videos.ToVideoModels().ToList(),
                    UserId = Settings.CurrentUserId
                };
                App.NoteRepository.Save(note);
                await NavigationService.NavigateBackAsync();
            }
            else
            {
                var note = App.NoteRepository.GetNoteAsync(_noteId);
                note.Description = Description;
                note.EditDate = DateTime.Now;
                note.Photos = Photos.ToPhotoModels().ToList();
                note.Videos = Videos.ToVideoModels().ToList();
                App.NoteRepository.Save(note);
            }
            IsLoading = false;
        }

        private async Task DeleteNote()
        {
            if (_noteId != 0)
            {
                bool result = await UserDialogs.Instance.ConfirmAsync(ConstantsHelper.NoteDeleteMessage,
                    ConstantsHelper.Warning, ConstantsHelper.Ok, ConstantsHelper.Cancel);
                if (result)
                {
                    var noteToDelete = App.NoteRepository.GetNoteAsync(_noteId);
                    App.NoteRepository.DeleteNote(noteToDelete);
                    await NavigationService.NavigateBackAsync();
                }
            }
        }
    }
}