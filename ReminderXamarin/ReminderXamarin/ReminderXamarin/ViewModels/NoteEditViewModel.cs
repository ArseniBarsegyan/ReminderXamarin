using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Acr.UserDialogs;

using ReminderXamarin.Collections;
using ReminderXamarin.Core.Interfaces;
using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Core.Interfaces.Commanding.AsyncCommanding;
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

        private readonly MediaHelper _mediaHelper;
        private readonly TransformHelper _transformHelper;

        private int _noteId;
        private Note _note;
        private bool _subscribed;

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

            _mediaHelper = mediaHelper;
            _transformHelper = transformHelper;

            AttachButtonImageSource = ConstantsHelper.AttachmentLightIcon;
            CameraButtonImageSource = ConstantsHelper.CameraIcon;
            VideoButtonImageSource = ConstantsHelper.VideoIcon;

            GalleryItemModels = new RangeObservableCollection<GalleryItemModel>();

            DescriptionTextChanged = commandResolver.Command<string>(DescriptionChanged);
            TakePhotoCommand = commandResolver.AsyncCommand(TakePhoto);
            TakeVideoCommand = commandResolver.AsyncCommand(TakeVideo);
            PickMultipleMediaCommand = commandResolver.AsyncCommand(PickMultipleMedia);
            SaveNoteCommand = commandResolver.AsyncCommand<string>(SaveNote);
            DeleteNoteCommand = commandResolver.AsyncCommand(DeleteNote);
            SelectImageCommand = commandResolver.AsyncCommand<GalleryItemModel>(SelectImage);
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

        public bool IsGalleryVisible => GalleryItemModels.Count > 0;

        public RangeObservableCollection<GalleryItemModel> GalleryItemModels { get; set; }

        public ICommand DescriptionTextChanged { get; }
        public IAsyncCommand TakePhotoCommand { get; }
        public IAsyncCommand TakeVideoCommand { get; }
        public IAsyncCommand PickMultipleMediaCommand { get; }
        public IAsyncCommand<string> SaveNoteCommand { get; }
        public IAsyncCommand DeleteNoteCommand { get; }
        public IAsyncCommand<GalleryItemModel> SelectImageCommand { get; }

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
            _noteId = (int)navigationData;

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
                GalleryItemModels.ReplaceRangeWithoutUpdating(_note.GalleryItems);
                GalleryItemModels.RaiseCollectionChanged();
                OnPropertyChanged(nameof(IsGalleryVisible));
            }
            return base.InitializeAsync(navigationData);
        }

        public void OnAppearing()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            if (_subscribed)
                return;

            MessagingCenter.Subscribe<GalleryItemViewModel, int>(this,
                ConstantsHelper.ImageDeleted, (vm, id) => DeletePhoto(id));
            IsToolbarItemVisible = false;
            GalleryItemModels.CollectionChanged += GalleryItemsViewModels_CollectionChanged;
            App.MultiMediaPickerService.OnMediaPickedCompleted += MultiMediaPickerServiceOnMediaPickedCompleted;
            _subscribed = true;
        }

        private void Unsubscribe()
        {
            MessagingCenter.Unsubscribe<GalleryItemViewModel>(this, ConstantsHelper.ImageDeleted);
            MessagingCenter.Send(this, ConstantsHelper.NoteEditPageDisappeared);
            GalleryItemModels.CollectionChanged -= GalleryItemsViewModels_CollectionChanged;
            _subscribed = false;
        }

        public void OnDisappearing()
        {
            Unsubscribe();
        }

        private void MultiMediaPickerServiceOnMediaPickedCompleted(object sender, IList<Services.MediaPicker.MediaFile> e)
        {
            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    IsLoading = true;
                });

                Parallel.ForEach(e, async(item) =>
                {
                    var model = await GetGalleryItemModelFromMediaFile(item).ConfigureAwait(false);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        GalleryItemModels.Add(model);
                    });
                });

                Device.BeginInvokeOnMainThread(() =>
                {
                    IsLoading = false;
                });

                App.MultiMediaPickerService.OnMediaPickedCompleted -= MultiMediaPickerServiceOnMediaPickedCompleted;
            });
        }

        private async Task<GalleryItemModel> GetGalleryItemModelFromMediaFile(Services.MediaPicker.MediaFile mediaFile)
        {
            var model = new GalleryItemModel
            {
                NoteId = _noteId
            };

            byte[] imageContent = _fileService.ReadAllBytes(mediaFile.Path);

            var imageName = Path.GetFileName(mediaFile.Path);

            byte[] resizedImage = _mediaService.ResizeImage(imageContent,
                ConstantsHelper.ResizedImageWidth,
                ConstantsHelper.ResizedImageHeight);

            string imagePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal), 
                imageName);

            try
            {
                File.WriteAllBytes(imagePath, resizedImage);
            }
            catch(Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message);
            }
            
            model.ImagePath = imagePath;
            model.Thumbnail = imagePath;

            await _transformHelper.ResizeAsync(imagePath, model).ConfigureAwait(false);

            return model;
        }

        public Task<bool> AskAboutLeave()
        {
            return UserDialogs.Instance.ConfirmAsync(
                ConstantsHelper.PageCloseMessage,
                ConstantsHelper.Warning,
                ConstantsHelper.Ok, 
                ConstantsHelper.Cancel);
        }

        private void GalleryItemsViewModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IsToolbarItemVisible = true;
            OnPropertyChanged(nameof(IsGalleryVisible));
        }

        private async Task PickMultipleMedia()
        {
            var hasPermission = await CheckPermissionsAsync();
            if (hasPermission)
            {
                await App.MultiMediaPickerService.PickPhotosAsync();
            }
        }

        private async Task SelectImage(GalleryItemModel viewModel)
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

        private void DeletePhoto(int id)
        {
            IsLoading = true;
            if (GalleryItemModels.Any())
            {
                var model = GalleryItemModels.FirstOrDefault(x => x.Id == id);
                if (model != null)
                {
                    GalleryItemModels.Remove(model);
                }
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
                    var model = await _mediaHelper.TakePhotoAsync();
                    if (model != null)
                    {
                        GalleryItemModels.Add(model);
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
                var model = await _mediaHelper.TakeVideoAsync().ConfigureAwait(false);

                if (model != null)
                {
                    model.NoteId = _noteId;
                    model.IsVideo = true;

                    var videoName =
                        model.VideoPath.Substring(
                            model.VideoPath.LastIndexOf(@"/", StringComparison.InvariantCulture) + 1);
                    var imageName = videoName.Substring(0, videoName.Length - 4) + "_thumb.jpg";

                    byte[] imageContent =
                        _mediaService.GenerateThumbImage(model.VideoPath,
                            ConstantsHelper.ThumbnailTimeFrame);

                    byte[] resizedImage = _mediaService.ResizeImage(imageContent,
                        ConstantsHelper.ResizedImageWidth,
                        ConstantsHelper.ResizedImageHeight);

                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    string imagePath = Path.Combine(path, imageName);

                    File.WriteAllBytes(imagePath, resizedImage);
                    model.ImagePath = imagePath;
                    model.Thumbnail = imagePath;

                    await _transformHelper.ResizeAsync(imagePath, model).ConfigureAwait(false);

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        GalleryItemModels.Add(model);
                    });
                }
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                IsLoading = false;
            });
        }

        private async Task SaveNote(string text)
        {
            ShouldPromptUser = false;
            Description = text;

            IsLoading = true;
            if (_noteId == 0)
            {
                await NavigationService.NavigateBackAsync();
                Task.Run(() =>
                {
                    var note = new Note
                    {
                        CreationDate = DateTime.Now,
                        EditDate = DateTime.Now,
                        Description = Description,
                        GalleryItems = GalleryItemModels.ToList(),
                        UserId = Settings.CurrentUserId
                    };
                    App.NoteRepository.Value.Save(note);
                    MessagingCenter.Send(this, ConstantsHelper.NoteCreated);
                });
            }
            else
            {
                var note = App.NoteRepository.Value.GetNoteAsync(_noteId);
                note.Description = Description;
                note.EditDate = DateTime.Now;
                note.GalleryItems = GalleryItemModels.ToList();
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
                bool result = await UserDialogs.Instance.ConfirmAsync(
                    ConstantsHelper.NoteDeleteMessage,
                    ConstantsHelper.Warning, 
                    ConstantsHelper.Ok, 
                    ConstantsHelper.Cancel);

                if (result)
                {
                    await NavigationService.NavigateBackAsync();
                    Task.Run(() =>
                    {
                        var noteToDelete = App.NoteRepository.Value.GetNoteAsync(_noteId);
                        App.NoteRepository.Value.DeleteNote(noteToDelete);
                        MessagingCenter.Send(this, ConstantsHelper.NoteDeleted, _noteId);
                    });
                }
            }
        }
    }
}