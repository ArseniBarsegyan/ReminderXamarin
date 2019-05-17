using System;
using ReminderXamarin.Elements;
using ReminderXamarin.Services.FilePickerService;
using ReminderXamarin.ViewModels;
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

        private void Confirm_OnClicked(object sender, EventArgs e)
        {
            if (BindingContext is NoteEditViewModel editViewModel)
            {
                ShouldPromptUser = false;
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

        private async void OnPickMediaButtonClicked(object sender, EventArgs e)
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
                viewModel.PickMediaCommand.Execute(document);
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

        private void ImageButton_OnClicked(object sender, EventArgs e)
        {
            if (sender is ImageButton button)
            {
                ResetButtons();
                button.BackgroundColor = Color.FromHex("#448AFF");

                if (BindingContext is NoteEditViewModel viewModel)
                {

                }
                Device.StartTimer(TimeSpan.FromSeconds(0.5), () =>
                {
                    ResetButtons();
                    return false;
                });
            }
        }

        private void ResetButtons()
        {
            VideoButton.BackgroundColor = Color.Transparent;
            CameraButton.BackgroundColor = Color.Transparent;
            PickButton.BackgroundColor = Color.Transparent;
        }
    }
}