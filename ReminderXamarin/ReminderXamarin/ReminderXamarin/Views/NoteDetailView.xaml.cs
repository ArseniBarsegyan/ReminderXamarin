using System;
using ReminderXamarin.Elements;
using Rm.Helpers;
using ReminderXamarin.Services;
using ReminderXamarin.Services.FilePickerService;
using ReminderXamarin.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoteDetailView : ContentPage
    {
        private static readonly IPlatformDocumentPicker DocumentPicker = DependencyService.Get<IPlatformDocumentPicker>();
        private readonly NoteViewModel _noteViewModel;
        private readonly ToolbarItem _confirmToolbarItem;

        public NoteDetailView(NoteViewModel noteViewModel)
        {
            InitializeComponent();
            BindingContext = noteViewModel;
            _noteViewModel = noteViewModel;

            Title = $"{noteViewModel.EditDate:d}";
            _confirmToolbarItem = new ToolbarItem {Order = ToolbarItemOrder.Primary, Icon = "confirm.png"};
            _confirmToolbarItem.Clicked += Confirm_OnClicked;
            ToolbarItems.Add(_confirmToolbarItem);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DescriptionEditor.Text = _noteViewModel.Description;
            _noteViewModel.PhotosCollectionChanged += NoteViewModelOnPhotosCollectionChanged;
            MessagingCenter.Subscribe<ImageGallery, int>(this, ConstantsHelper.ImageDeleted, (gallery, i) =>
            {
                _noteViewModel.DeletePhotoCommand.Execute(i);
            });
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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _noteViewModel.PhotosCollectionChanged -= NoteViewModelOnPhotosCollectionChanged;
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

        private async void Delete_OnClicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert
                (ConstantsHelper.Warning, ConstantsHelper.NoteDeleteMessage, ConstantsHelper.Ok, ConstantsHelper.Cancel);
            if (result)
            {
                _noteViewModel.DeleteNoteCommand.Execute(null);
                await Navigation.PopAsync();
            }
        }

        private void Confirm_OnClicked(object sender, EventArgs e)
        {
            _noteViewModel.Description = DescriptionEditor.Text;
            _noteViewModel.UpdateNoteCommand.Execute(null);

            if (ToolbarItems.Contains(_confirmToolbarItem))
            {
                ToolbarItems.Remove(_confirmToolbarItem);
            }
        }

        private void DescriptionEditor_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!ToolbarItems.Contains(_confirmToolbarItem))
            {
                ToolbarItems.Add(_confirmToolbarItem);
            }
        }

        private async void HorizontalImageGallery_OnItemTapped(object sender, EventArgs e)
        {
            if (sender is Image tappedImage)
            {
                await Navigation.PushPopupAsync(new FullSizeImageView(tappedImage.Source));
            }
        }

        private void AddButton_OnClicked(object sender, EventArgs e)
        {
            AdditionalItemsContentView.IsVisible = true;
        }

        private async void AdditionalItemsContentView_OnPickPhotoButtonClicked(object sender, EventArgs e)
        {
            _noteViewModel.IsLoading = true;

            var document = await DocumentPicker.DisplayImportAsync(this);
            if (document == null)
            {
                _noteViewModel.IsLoading = false;
                return;
            }
            _noteViewModel.PickPhotoCommand.Execute(document);
        }

        private void AdditionalItemsContentView_OnTakePhotoButtonClicked(object sender, EventArgs e)
        {
            _noteViewModel.TakePhotoCommand.Execute(null);
        }

        private void AdditionalItemsContentView_OnTakeVideoButtonClicked(object sender, EventArgs e)
        {
            _noteViewModel.TakeVideoCommand.Execute(null);
        }

        private async void AdditionalItemsContentView_OnPickVideoButtonClicked(object sender, EventArgs e)
        {
            _noteViewModel.IsLoading = true;
            var document = await DocumentPicker.DisplayImportAsync(this);
            if (document == null)
            {
                _noteViewModel.IsLoading = false;
                return;
            }
            _noteViewModel.PickVideoCommand.Execute(document);
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
    }
}