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
    }
}