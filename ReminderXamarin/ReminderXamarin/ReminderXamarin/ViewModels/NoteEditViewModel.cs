using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using ReminderXamarin.Extensions;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ReminderXamarin.Helpers;
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
            GalleryItemsViewModels = new ObservableCollection<GalleryItemViewModel>();

            TakePhotoCommand = new Command(async () => await TakePhoto());
            DeletePhotoCommand = new Command<int>(DeletePhoto);
            TakeVideoCommand = new Command(async () => await TakeVideo());
            PickMediaCommand = new Command<PlatformDocument>(async document => await PickDocument(document));
            SaveNoteCommand = new Command(async() => await SaveNote());
            DeleteNoteCommand = new Command(async () => await DeleteNote());
            SelectImageCommand = new Command<GalleryItemViewModel>(async (viewModel) => await SelectImage(viewModel));
        }

        private async Task SelectImage(GalleryItemViewModel viewModel)
        {
            if (viewModel.IsVideo)
            {
                if (!string.IsNullOrWhiteSpace(viewModel.VideoPath))
                {
                    var videoService = DependencyService.Get<IVideoService>();
                    videoService.PlayVideo(viewModel.VideoPath);
                }
            }
            else
            {
                await NavigationService.NavigateToPopupAsync<GalleryItemViewModel>(viewModel);
            }
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
                GalleryItemsViewModels = _note.GalleryItems.ToViewModels();
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
        public ObservableCollection<GalleryItemViewModel> GalleryItemsViewModels { get; set; }
        
        public ICommand DeletePhotoCommand { get; set; }
        public ICommand TakePhotoCommand { get; set; }
        public ICommand TakeVideoCommand { get; set; }
        public ICommand PickPhotoCommand { get; set; }
        public ICommand PickMediaCommand { get; set; }
        public ICommand PickVideoCommand { get; set; }
        public ICommand SaveNoteCommand { get; set; }
        public ICommand DeleteNoteCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }

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
                        GalleryItemsViewModels.Add(photoModel.ToViewModel());
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

        private async Task PickDocument(PlatformDocument document)
        {
            if (document.Name.EndsWith(".png") || document.Name.EndsWith(".jpg") || document.Name.EndsWith(".jpeg"))
            {
                await PickPhoto(document);
            }
            else if (document.Name.EndsWith(".mp4"))
            {
                await PickVideo(document);
            }
        }

        private async Task PickPhoto(PlatformDocument document)
        {
            IsLoading = true;
            try
            {
                var galleryItemModel = new GalleryItemModel
                {
                    NoteId = _noteId
                };

                var mediaService = DependencyService.Get<IMediaService>();
                var fileSystem = DependencyService.Get<IFileSystem>();
                var imageContent = fileSystem.ReadAllBytes(document.Path);

                var resizedImage = mediaService.ResizeImage(imageContent, ConstantsHelper.ResizedImageWidth,
                    ConstantsHelper.ResizedImageHeight);
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string imagePath = Path.Combine(path, document.Name);

                File.WriteAllBytes(imagePath, resizedImage);
                galleryItemModel.ImagePath = imagePath;
                galleryItemModel.Thumbnail = imagePath;

                await _transformHelper.ResizeAsync(imagePath, galleryItemModel);

                GalleryItemsViewModels.Add(galleryItemModel.ToViewModel());
                PhotosCollectionChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
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
                        videoModel.NoteId = _noteId;
                        videoModel.IsVideo = true;

                        var videoName =
                            videoModel.VideoPath.Substring(
                                videoModel.VideoPath.LastIndexOf(@"/", StringComparison.InvariantCulture) + 1);
                        var imageName = videoName.Substring(0, videoName.Length - 4) + "_thumb.jpg";

                        var mediaService = DependencyService.Get<IMediaService>();
                        var imageContent =
                            mediaService.GenerateThumbImage(videoModel.VideoPath, ConstantsHelper.ThumbnailTimeFrame);

                        var resizedImage = mediaService.ResizeImage(imageContent, ConstantsHelper.ResizedImageWidth,
                            ConstantsHelper.ResizedImageHeight);
                        string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        string imagePath = Path.Combine(path, imageName);

                        File.WriteAllBytes(imagePath, resizedImage);
                        videoModel.ImagePath = imagePath;
                        videoModel.Thumbnail = imagePath;

                        await _transformHelper.ResizeAsync(imagePath, videoModel);

                        GalleryItemsViewModels.Add(videoModel.ToViewModel());
                        PhotosCollectionChanged?.Invoke(this, EventArgs.Empty);
                    }
                    catch (Exception ex)
                    {
                        await UserDialogs.Instance.AlertAsync(ex.Message);
                    }
                }
            }
            IsLoading = false;
        }

        private async Task PickVideo(PlatformDocument document)
        {
            IsLoading = true;
            try
            {
                var galleryItemModel = new GalleryItemModel
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

                galleryItemModel.VideoPath = videoPath;
                galleryItemModel.ImagePath = imagePath;
                galleryItemModel.Thumbnail = imagePath;

                await _transformHelper.ResizeAsync(imagePath, galleryItemModel);
                
                GalleryItemsViewModels.Add(galleryItemModel.ToViewModel());
                PhotosCollectionChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
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
                    GalleryItems = GalleryItemsViewModels.ToModels().ToList(),
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
                note.GalleryItems = GalleryItemsViewModels.ToModels().ToList();
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