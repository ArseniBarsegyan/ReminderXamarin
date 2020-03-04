using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Acr.UserDialogs;

using ReminderXamarin.Core.Interfaces;
using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Core.Interfaces.Commanding.AsyncCommanding;
using ReminderXamarin.Extensions;
using ReminderXamarin.Helpers;
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
        private readonly ICommandResolver _commandResolver;

        private readonly MediaHelper _mediaHelper;
        private readonly TransformHelper _transformHelper;

        private int _noteId;
        private Note _note;

        public NoteEditViewModel(INavigationService navigationService,
            IPermissionService permissionService,
            IFileSystem fileService,
            IMediaService mediaService,
            IVideoService videoService,
            ICommandResolver commandResolver,
            MediaHelper mediaHelper,
            TransformHelper transformHelper)
            : base(navigationService)
        {
            _permissionService = permissionService;
            _fileService = fileService;
            _mediaService = mediaService;
            _videoService = videoService;
            _commandResolver = commandResolver;

            _mediaHelper = mediaHelper;
            _transformHelper = transformHelper;

            AttachButtonImageSource = ConstantsHelper.AttachmentLightIcon;
            CameraButtonImageSource = ConstantsHelper.CameraIcon;
            VideoButtonImageSource = ConstantsHelper.VideoIcon;

            GalleryItemsViewModels = new ObservableCollection<GalleryItemViewModel>();            

            DescriptionTextChanged = commandResolver.Command<string>(DescriptionChanged);
            TakePhotoCommand = commandResolver.AsyncCommand(TakePhoto);
            DeletePhotoCommand = commandResolver.Command<GalleryItemViewModel>(DeletePhoto);
            TakeVideoCommand = commandResolver.AsyncCommand(TakeVideo);
            PickMultipleMediaCommand = commandResolver.AsyncCommand(PickMultipleMedia);
            SaveNoteCommand = commandResolver.AsyncCommand<string>(SaveNote);
            DeleteNoteCommand = commandResolver.AsyncCommand(DeleteNote);
            SelectImageCommand = commandResolver.AsyncCommand<GalleryItemViewModel>(SelectImage);
        }        

        public ImageSource AttachButtonImageSource { get; private set; }
        public ImageSource CameraButtonImageSource { get; private set; }
        public ImageSource VideoButtonImageSource { get; private set; }

        public bool IsToolbarItemVisible { get; set; }

        public string Title { get; set; }
        public bool IsLoading { get; set; }
        public bool IsEditMode { get; set; }
        public string Description { get; set; }
        public bool ShouldPromptUser { get; set; }
        public ObservableCollection<GalleryItemViewModel> GalleryItemsViewModels { get; set; }

        public ICommand DescriptionTextChanged { get; }
        public IAsyncCommand TakePhotoCommand { get; }
        public ICommand DeletePhotoCommand { get; }        
        public IAsyncCommand TakeVideoCommand { get; }
        public IAsyncCommand PickMultipleMediaCommand { get; }
        public IAsyncCommand<string> SaveNoteCommand { get; }
        public IAsyncCommand DeleteNoteCommand { get; }
        public IAsyncCommand<GalleryItemViewModel> SelectImageCommand { get; }

        private void DescriptionChanged(string value)
        {
            if (!IsEditMode)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    ShouldPromptUser = true;
                }
            }
            IsToolbarItemVisible = value != Description;           
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
                GalleryItemsViewModels = _note.GalleryItems.ToViewModels(NavigationService, _commandResolver);
            }
            return base.InitializeAsync(navigationData);
        }

        public void OnAppearing()
        {
            MessagingCenter.Subscribe<GalleryItemViewModel>(this,
                ConstantsHelper.ImageDeleted, DeletePhoto);
            IsToolbarItemVisible = false;
            GalleryItemsViewModels.CollectionChanged += GalleryItemsViewModels_CollectionChanged;
        }

        public void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<GalleryItemViewModel>(this, ConstantsHelper.ImageDeleted);
            MessagingCenter.Send(this, ConstantsHelper.NoteEditPageDisappeared);
            GalleryItemsViewModels.CollectionChanged -= GalleryItemsViewModels_CollectionChanged;
        }

        public Task<bool> AskAboutLeave()
        {
            return UserDialogs.Instance.ConfirmAsync(ConstantsHelper.PageCloseMessage,
                ConstantsHelper.Warning,
                ConstantsHelper.Ok, ConstantsHelper.Cancel);
        }

        private void GalleryItemsViewModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IsToolbarItemVisible = true;
        }

        private async Task PickMultipleMedia()
        {
            var multipleMediaPickerService = App.MultiMediaPickerService;
            multipleMediaPickerService.OnMediaPicked += (sender, file) =>
            {
                Task.Run(async() => 
                {
                    var galleryItemModel = new GalleryItemModel
                    {
                        NoteId = _noteId
                    };
                    var imageContent = _fileService.ReadAllBytes(file.Path);
                    var imageName = Path.GetFileName(file.Path);

                    var resizedImage = _mediaService.ResizeImage(imageContent, 
                        ConstantsHelper.ResizedImageWidth,
                        ConstantsHelper.ResizedImageHeight);

                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    string imagePath = Path.Combine(path, imageName);

                    File.WriteAllBytes(imagePath, resizedImage);
                    galleryItemModel.ImagePath = imagePath;
                    galleryItemModel.Thumbnail = imagePath;

                    await _transformHelper.ResizeAsync(imagePath, galleryItemModel).ConfigureAwait(false);

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        GalleryItemsViewModels.Add(galleryItemModel.ToViewModel
                            (NavigationService, _commandResolver));
                    });
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
                await NavigationService.NavigateToPopupAsync<GalleryItemViewModel>(viewModel).ConfigureAwait(false);
            }
        }

        private void DeletePhoto(GalleryItemViewModel viewModel)
        {
            IsLoading = true;
            if (GalleryItemsViewModels.Any())
            {
                GalleryItemsViewModels.Remove(viewModel);
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
                        GalleryItemsViewModels.Add(photoModel.ToViewModel(NavigationService, _commandResolver));                        
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
                    await Task.Run(async () =>
                    {
                        videoModel.NoteId = _noteId;
                        videoModel.IsVideo = true;

                        var videoName =
                            videoModel.VideoPath.Substring(
                                videoModel.VideoPath.LastIndexOf(@"/", StringComparison.InvariantCulture) + 1);
                        var imageName = videoName.Substring(0, videoName.Length - 4) + "_thumb.jpg";

                        var imageContent =
                            _mediaService.GenerateThumbImage(videoModel.VideoPath,
                                ConstantsHelper.ThumbnailTimeFrame);

                        var resizedImage = _mediaService.ResizeImage(imageContent,
                            ConstantsHelper.ResizedImageWidth,
                            ConstantsHelper.ResizedImageHeight);

                        string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        string imagePath = Path.Combine(path, imageName);

                        File.WriteAllBytes(imagePath, resizedImage);
                        videoModel.ImagePath = imagePath;
                        videoModel.Thumbnail = imagePath;

                        await _transformHelper.ResizeAsync(imagePath, videoModel).ConfigureAwait(false);

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            GalleryItemsViewModels.Add(videoModel.ToViewModel(NavigationService, _commandResolver));
                        });
                    });
                }
            }
            IsLoading = false;
        }

        private async Task SaveNote(string text)
        {
            ShouldPromptUser = false;
            Description = text;

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
                await NavigationService.NavigateBackAsync().ConfigureAwait(false);
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

            IsToolbarItemVisible = false;
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
                    await NavigationService.NavigateBackAsync().ConfigureAwait(false);
                }
            }
        }
    }
}