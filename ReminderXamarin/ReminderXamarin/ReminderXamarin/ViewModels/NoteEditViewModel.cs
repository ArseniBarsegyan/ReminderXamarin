using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Acr.UserDialogs;

using ReminderXamarin.DependencyResolver;
using ReminderXamarin.Extensions;
using ReminderXamarin.Helpers;
using ReminderXamarin.Services;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

using Rm.Data.Data.Entities;
using Rm.Helpers;

using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class NoteEditViewModel : BaseViewModel
    {
        private readonly IPermissionService _permissionService;
        private readonly IFileSystem _fileService;
        private readonly IMediaService _mediaService;
        private readonly IVideoService _videoService;

        // TODO: register as dependency
        private readonly MediaHelper _mediaHelper;
        private readonly TransformHelper _transformHelper;

        private int _noteId;
        private Note _note;

        public NoteEditViewModel(INavigationService navigationService,
            IPermissionService permissionService,
            IFileSystem fileService,
            IMediaService mediaService,
            IVideoService videoService)
            : base(navigationService)
        {
            _permissionService = permissionService;
            _fileService = fileService;
            _mediaService = mediaService;
            _videoService = videoService;

            _mediaHelper = new MediaHelper();
            _transformHelper = new TransformHelper();
            
            GalleryItemsViewModels = new ObservableCollection<GalleryItemViewModel>();
            
            TakePhotoCommand = new Command(async () => await TakePhoto());
            DeletePhotoCommand = new Command<GalleryItemViewModel>(DeletePhoto);
            TakeVideoCommand = new Command(async () => await TakeVideo());
            PickMultipleMediaCommand = new Command(async () => await PickMultipleMedia());
            SaveNoteCommand = new Command(async() => await SaveNote());
            DeleteNoteCommand = new Command(async () => await DeleteNote());
            SelectImageCommand = new Command<GalleryItemViewModel>(async viewModel =>
                await SelectImage(viewModel));
        }
        
        public void OnAppearing()
        {
            MessagingCenter.Subscribe<GalleryItemViewModel>(this, 
                ConstantsHelper.ImageDeleted, DeletePhoto);
        }

        public void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<GalleryItemViewModel>(this, ConstantsHelper.ImageDeleted);
            MessagingCenter.Send(this, ConstantsHelper.NoteEditPageDisappeared);
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
                _note = App.NoteRepository.Value.GetNoteAsync(_noteId);
                Title = _note.EditDate.ToString("d");
                Description = _note.Description;
                GalleryItemsViewModels = _note.GalleryItems.ToViewModels(NavigationService);
                PhotosCollectionChanged?.Invoke(this, EventArgs.Empty);
            }
            return base.InitializeAsync(navigationData);
        }

        public bool IsEditMode { get; set; }
        public string Title { get; set; }
        public bool IsLoading { get; set; }
        public string Description { get; set; }
        public ObservableCollection<GalleryItemViewModel> GalleryItemsViewModels { get; set; }
        
        public ICommand DeletePhotoCommand { get; }
        public ICommand TakePhotoCommand { get; }
        public ICommand TakeVideoCommand { get; }
        public ICommand PickMultipleMediaCommand { get; }
        public ICommand SaveNoteCommand { get; }
        public ICommand DeleteNoteCommand { get; }
        public ICommand SelectImageCommand { get; }

        public event EventHandler PhotosCollectionChanged;

        private async Task PickMultipleMedia()
        {
            var multipleMediaPickerService = App.MultiMediaPickerService;
            multipleMediaPickerService.OnMediaPicked += (sender, file) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var galleryItemModel = new GalleryItemModel
                    {
                        NoteId = _noteId
                    };
                    var imageContent = _fileService.ReadAllBytes(file.Path);
                    var imageName = Path.GetFileName(file.Path);

                    var resizedImage = _mediaService.ResizeImage(imageContent, ConstantsHelper.ResizedImageWidth,
                        ConstantsHelper.ResizedImageHeight);
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    string imagePath = Path.Combine(path, imageName);

                    File.WriteAllBytes(imagePath, resizedImage);
                    galleryItemModel.ImagePath = imagePath;
                    galleryItemModel.Thumbnail = imagePath;

                    await _transformHelper.ResizeAsync(imagePath, galleryItemModel);

                    GalleryItemsViewModels.Add(galleryItemModel.ToViewModel(NavigationService));
                    PhotosCollectionChanged?.Invoke(this, EventArgs.Empty);
                });
            };

            var hasPermission = await CheckPermissionsAsync();
            if (hasPermission)
            {
                await multipleMediaPickerService.PickPhotosAsync();
            }
        }

        private async Task SelectImage(GalleryItemViewModel viewModel)
        {
            if (viewModel.IsVideo)
            {
                if (!string.IsNullOrWhiteSpace(viewModel.VideoPath))
                {
                    _videoService.PlayVideo(viewModel.VideoPath);
                }
            }
            else
            {
                await NavigationService.NavigateToPopupAsync<GalleryItemViewModel>(viewModel);
            }
        }

        private void DeletePhoto(GalleryItemViewModel viewModel)
        {
            IsLoading = true;
            if (GalleryItemsViewModels.Any())
            {
                GalleryItemsViewModels.Remove(viewModel);
                PhotosCollectionChanged?.Invoke(this, EventArgs.Empty);
            }
            IsLoading = false;
        }

        private async Task TakePhoto()
        {
            bool permissionResult = await _permissionService.AskPermission();
            if (permissionResult)
            {
                IsLoading = true;
                try
                {
                    var photoModel = await _mediaHelper.TakePhotoAsync();
                    if (photoModel != null)
                    {
                        GalleryItemsViewModels.Add(photoModel.ToViewModel(NavigationService));
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

        private async Task TakeVideo()
        {
            bool permissionResult = await _permissionService.AskPermission();

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

                        var mediaService = ComponentFactory.Resolve<IMediaService>();
                        var imageContent =
                            mediaService.GenerateThumbImage(videoModel.VideoPath, 
                                ConstantsHelper.ThumbnailTimeFrame);

                        var resizedImage = mediaService.ResizeImage(imageContent, 
                            ConstantsHelper.ResizedImageWidth,
                            ConstantsHelper.ResizedImageHeight);
                        string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        string imagePath = Path.Combine(path, imageName);

                        File.WriteAllBytes(imagePath, resizedImage);
                        videoModel.ImagePath = imagePath;
                        videoModel.Thumbnail = imagePath;

                        await _transformHelper.ResizeAsync(imagePath, videoModel);

                        GalleryItemsViewModels.Add(videoModel.ToViewModel(NavigationService));
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
                App.NoteRepository.Value.Save(note);
                MessagingCenter.Send(this, ConstantsHelper.NoteCreated);
                await NavigationService.NavigateBackAsync();
            }
            else
            {
                var note = App.NoteRepository.Value.GetNoteAsync(_noteId);
                note.Description = Description;
                note.EditDate = DateTime.Now;
                note.GalleryItems = GalleryItemsViewModels.ToModels().ToList();
                App.NoteRepository.Value.Save(note);
                MessagingCenter.Send(this, ConstantsHelper.NoteEdited, _noteId);
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
                    var noteToDelete = App.NoteRepository.Value.GetNoteAsync(_noteId);
                    App.NoteRepository.Value.DeleteNote(noteToDelete);
                    MessagingCenter.Send(this, ConstantsHelper.NoteDeleted, _noteId);
                    await NavigationService.NavigateBackAsync();
                }
            }
        }
    }
}