using ReminderXamarin.ViewModels;

using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoteEditView : ContentPage
    {
        private bool _isAnimationInProgress;

        public NoteEditView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is NoteEditViewModel viewModel)
            {
                viewModel.OnAppearing();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (BindingContext is NoteEditViewModel viewModel)
            {
                viewModel.OnDisappearing();
            }
        }

        private async void ToggleOptionsLayout(object sender, System.EventArgs e)
        {
            if (_isAnimationInProgress)
            {
                return;
            }
            _isAnimationInProgress = true;

            if (AttachOptionLayout.IsVisible)
            {
                await HideOptionsLayout();
            }
            else
            {
                await ShowOptionsLayout();
            }

            _isAnimationInProgress = false;
        }

        private async Task ShowOptionsLayout()
        {
            await AttachButton.TranslateTo(0, 240, 0);
            await CameraButton.TranslateTo(0, 180, 0);
            await VideoButton.TranslateTo(0, 120, 0);

            AttachOptionLayout.IsVisible = true;
            var tasks = new Task[]
            {
                AttachButton.TranslateTo(0, 0, 300),
                CameraButton.TranslateTo(0, 0, 300),
                VideoButton.TranslateTo(0, 0, 300),
                ShowOptionsLayoutButton.RotateTo(45, 200),
                AttachOptionLayout.FadeTo(1, 200)
            };
            await Task.WhenAll(tasks);
        }

        private async Task HideOptionsLayout()
        {
            var tasks = new Task[]
            {
                ShowOptionsLayoutButton.RotateTo(0, 200),
                AttachOptionLayout.FadeTo(0, 200)
            };
            await Task.WhenAll(tasks);
            AttachOptionLayout.IsVisible = false;
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            await AnimateGalleryShow();
        }

        private async void DescriptionEditor_Focused(object sender, FocusEventArgs e)
        {
            await AnimateGalleryHide();
        }        

        private async void DescriptionEditor_Unfocused(object sender, FocusEventArgs e)
        {
            await AnimateGalleryShow();
        }

        private async Task AnimateGalleryHide()
        {
            await GalleryCarousel.TranslateTo(0, -300, 150);
            await ShowOptionsLayoutButton.TranslateTo(0, -250, 150);
            await AttachOptionLayout.TranslateTo(0, -250, 150);
            GalleryCarousel.IsVisible = false;
        }

        private async Task AnimateGalleryShow()
        {
            GalleryCarousel.IsVisible = true;
            await GalleryCarousel.TranslateTo(0, 0, 150);
            await ShowOptionsLayoutButton.TranslateTo(0, 0, 150);
            await AttachOptionLayout.TranslateTo(0, 0, 150);
        }
    }
}