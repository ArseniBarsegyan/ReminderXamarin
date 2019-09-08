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
        private static readonly IFileSystem FileService = DependencyService.Get<IFileSystem>();
        private static readonly IMediaService MediaService = DependencyService.Get<IMediaService>();

        public NoteEditViewModel()
        {
            _mediaHelper = new MediaHelper();
            _transformHelper = new TransformHelper();
            
            GalleryItemsViewModels = new ObservableCollection<GalleryItemViewModel>();
            
            TakePhotoCommand = new Command(async () => await TakePhoto());
            DeletePhotoCommand = new Command<GalleryItemViewModel>(vm => DeletePhoto(vm));
            TakeVideoCommand = new Command(async () => await TakeVideo());
            PickMultipleMediaCommand = new Command(async () => await PickMultipleMedia());
            SaveNoteCommand = new Command(async() => await SaveNote());
            DeleteNoteCommand = new Command(async () => await DeleteNote());
            SelectImageCommand = new Command<GalleryItemViewModel>(async viewModel => await SelectImage(viewModel));
        }
        
        public void OnAppearing()
        {
            MessagingCenter.Subscribe<GalleryItemViewModel>(this, ConstantsHelper.ImageDeleted, (vm) => DeletePhoto(vm));
        }

        public void OnDissapearing()
        {
            MessagingCenter.Unsubscribe<GalleryItemViewModel>(this, ConstantsHelper.ImageDeleted);
            MessagingCenter.Send(this, ConstantsHelper.NoteEditPageDissapeared);
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
                GalleryItemsViewModels = _note.GalleryItems.ToViewModels();
                PhotosCollectionChanged?.Invoke(this, EventArgs.Empty);
            }
            return base.InitializeAsync(navigationData);
        }

        public bool IsEditMode { get; set; }
        public string Title { get; set; }
        public bool IsLoading { get; set; }
        public string Description { get; set; }
        public ObservableCollection<GalleryItemViewModel> GalleryItemsViewModels { get; set; }
        
        public ICommand DeletePhotoCommand { get; set; }
        public ICommand TakePhotoCommand { get; set; }
        public ICommand TakeVideoCommand { get; set; }
        public ICommand PickMultipleMediaCommand { get; set; }
        public ICommand SaveNoteCommand { get; set; }
        public ICommand DeleteNoteCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }

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
                    var imageContent = FileService.ReadAllBytes(file.Path);
                    var imageName = Path.GetFileName(file.Path);

                    var resizedImage = MediaService.ResizeImage(imageContent, ConstantsHelper.ResizedImageWidth,
                        ConstantsHelper.ResizedImageHeight);
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    string imagePath = Path.Combine(path, imageName);

                    File.WriteAllBytes(imagePath, resizedImage);
                    galleryItemModel.ImagePath = imagePath;
                    galleryItemModel.Thumbnail = imagePath;

                    await _transformHelper.ResizeAsync(imagePath, galleryItemModel);

                    GalleryItemsViewModels.Add(galleryItemModel.ToViewModel());
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
                    var videoService = DependencyService.Get<IVideoService>();
                    videoService.PlayVideo(viewModel.VideoPath);
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