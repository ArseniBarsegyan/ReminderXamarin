using System;
using System.Collections.ObjectModel;
using System.Linq;
using ReminderXamarin.Elements;
using ReminderXamarin.Services;
using ReminderXamarin.Services.FilePickerService;
using ReminderXamarin.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Rm.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoteEditView : ContentPage
    {
        private static readonly IPlatformDocumentPicker DocumentPicker = DependencyService.Get<IPlatformDocumentPicker>();
        private readonly ToolbarItem _confirmToolbarItem;

        public NoteEditView()
        {
            InitializeComponent();
            _confirmToolbarItem = new ToolbarItem { Order = ToolbarItemOrder.Primary, Icon = "confirm.png" };
            _confirmToolbarItem.Clicked += Confirm_OnClicked;
            ToolbarItems.Add(_confirmToolbarItem);
        }

        public bool ShouldPromptUser { get; private set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is NoteEditViewModel noteEditViewModel)
            {
                if (!noteEditViewModel.IsEditMode)
                {
                    ToolbarItems.Remove(DeleteOption);
                }
                if (!string.IsNullOrEmpty(noteEditViewModel.Description))
                {
                    DescriptionEditor.Text = noteEditViewModel.Description;
                }
                noteEditViewModel.PhotosCollectionChanged += NoteViewModelOnPhotosCollectionChanged;
                MessagingCenter.Subscribe<ImageGallery, int>(this, ConstantsHelper.ImageDeleted, (gallery, i) =>
                {
                    noteEditViewModel.DeletePhotoCommand.Execute(i);
                });
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (BindingContext is NoteEditViewModel noteEditViewModel)
            {
                noteEditViewModel.PhotosCollectionChanged -= NoteViewModelOnPhotosCollectionChanged;
            }
            MessagingCenter.Unsubscribe<ImageGallery, int>(this, ConstantsHelper.ImageDeleted);
        }

        // Tap on back should close popup first
        protected override bool OnBackButtonPressed()
        {
            if (AdditionalItemsContentView.IsVisible)
            {
                AdditionalItemsContentView.IsVisible = false;
                return true;
            }
            return base.OnBackButtonPressed();
        }

        private void Confirm_OnClicked(object sender, EventArgs e)
        {
            if (BindingContext is NoteEditViewModel editViewModel)
            {
                editViewModel.Description = DescriptionEditor.Text;
                editViewModel.SaveNoteCommand.Execute(null);

                if (ToolbarItems.Contains(_confirmToolbarItem))
                {
                    ToolbarItems.Remove(_confirmToolbarItem);
                }
            }
        }

        private void NoteViewModelOnPhotosCollectionChanged(object sender, EventArgs eventArgs)
        {
            ImageGallery.IsVisible = true;
            ImageGallery.Render();

            if (!ToolbarItems.Contains(_confirmToolbarItem))
            {
                ToolbarItems.Add(_confirmToolbarItem);
            }
        }

        // TODO: replace with photo view model and ability to delete it
        private async void HorizontalImageGallery_OnItemTapped(object sender, EventArgs e)
        {
            if (sender is Image tappedImage)
            {
                await Navigation.PushPopupAsync(new FullSizeImageView(tappedImage.Source));
            }
        }

        private void VideoList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var viewModel = e.SelectedItem as VideoViewModel;
            VideoList.SelectedItem = null;
            if (viewModel != null)
            {
                var videoService = DependencyService.Get<IVideoService>();
                videoService.PlayVideo(viewModel.Path);
            }
        }

        private void AddButton_OnClicked(object sender, EventArgs e)
        {
            AdditionalItemsContentView.IsVisible = true;
        }

        // TODO: replace events with commands
        private async void AdditionalItemsContentView_OnPickPhotoButtonClicked(object sender, EventArgs e)
        {
            if (BindingContext is NoteEditViewModel viewModel)
            {
                viewModel.IsLoading = true;

                var document = await DocumentPicker.DisplayImportAsync(this);
                if (document == null)
                {
                    viewModel.IsLoading = false;
                    return;
                }
                viewModel.PickPhotoCommand.Execute(document);
            }
        }

        private void AdditionalItemsContentView_OnTakePhotoButtonClicked(object sender, EventArgs e)
        {
            if (BindingContext is NoteEditViewModel viewModel)
            {
                viewModel.TakePhotoCommand.Execute(null);
            }
        }

        private void AdditionalItemsContentView_OnTakeVideoButtonClicked(object sender, EventArgs e)
        {
            if (BindingContext is NoteEditViewModel viewModel)
            {
                viewModel.TakeVideoCommand.Execute(null);
            }
        }

        private async void AdditionalItemsContentView_OnPickVideoButtonClicked(object sender, EventArgs e)
        {
            if (BindingContext is NoteEditViewModel viewModel)
            {
                viewModel.IsLoading = true;
                var document = await DocumentPicker.DisplayImportAsync(this);
                if (document == null)
                {
                    viewModel.IsLoading = false;
                    return;
                }
                viewModel.PickVideoCommand.Execute(document);
            }
        }

        private void DescriptionEditor_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (BindingContext is NoteEditViewModel viewModel)
            {
                // Prompt only if create new note
                if (!viewModel.IsEditMode)
                {
                    if (DescriptionEditor.Text != string.Empty)
                    {
                        ShouldPromptUser = true;
                    }
                }
                
                if (!ToolbarItems.Contains(_confirmToolbarItem))
                {
                    ToolbarItems.Add(_confirmToolbarItem);
                }
            }
        }
    }
}