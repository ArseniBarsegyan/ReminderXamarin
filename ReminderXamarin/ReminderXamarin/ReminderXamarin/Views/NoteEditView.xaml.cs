using System;
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
            _confirmToolbarItem = new ToolbarItem { Order = ToolbarItemOrder.Primary, IconImageSource = ConstantsHelper.ConfirmIcon };
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
                noteEditViewModel.OnAppearing();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (BindingContext is NoteEditViewModel noteEditViewModel)
            {
                noteEditViewModel.PhotosCollectionChanged -= NoteViewModelOnPhotosCollectionChanged;
                noteEditViewModel.OnDissapearing();
            }
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
            if (!ToolbarItems.Contains(_confirmToolbarItem))
            {
                ToolbarItems.Add(_confirmToolbarItem);
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
                
                Device.StartTimer(TimeSpan.FromSeconds(0.5), () =>
                {
                    ResetButtons();
                    return false;
                });
            }
        }

        private void ResetButtons()
        {
            PickMultipleMediaButton.BackgroundColor = Color.Transparent;
            VideoButton.BackgroundColor = Color.Transparent;
            CameraButton.BackgroundColor = Color.Transparent;
        }
    }
}