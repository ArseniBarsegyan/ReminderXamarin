using System.Threading.Tasks;

using ReminderXamarin.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.Views
{
    [Preserve(AllMembers = true)]
    public partial class NoteEditView : ContentPage
    {
        private bool _isAnimationInProgress;
        private NoteEditViewModel ViewModel => BindingContext as NoteEditViewModel;

        public NoteEditView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel.OnDisappearing();
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
            await AttachButton.TranslateTo(0, 50, 0);
            await CameraButton.TranslateTo(50, 25, 0);
            await VideoButton.TranslateTo(50, 0, 0);

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

        private void DescriptionEditor_Focused(object sender, FocusEventArgs e)
        {
            ShowOptionsLayoutButton.IsVisible = !ShowOptionsLayoutButton.IsVisible;
        }
    }
}