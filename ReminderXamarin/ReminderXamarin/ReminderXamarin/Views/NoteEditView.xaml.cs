using ReminderXamarin.ViewModels;

using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoteEditView : ContentPage
    {
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
            if (AttachOptionLayout.IsVisible)
            {
                await HideOptionsLayout();
            }
            else
            {
                await ShowOptionsLayout();
            }
        }

        private async Task ShowOptionsLayout()
        {
            //var startPosition = ShowOptionsLayoutButton.GetScreenCoordinates();
            //var attachButtonEndPosition = AttachButton.GetScreenCoordinates();
            //var cameraButtonEndPosition = CameraButton.GetScreenCoordinates();
            //var videoButtonEndPosition = VideoButton.GetScreenCoordinates();

            //await AttachButton.TranslateTo(startPosition.X, startPosition.Y, 0);
            //await CameraButton.TranslateTo(startPosition.X, startPosition.Y, 0);
            //await VideoButton.TranslateTo(startPosition.X, startPosition.Y, 0);

            AttachOptionLayout.IsVisible = true;
            var tasks = new Task[]
            {
                //AttachButton.TranslateTo(attachButtonEndPosition.X, attachButtonEndPosition.Y, 400),
                //CameraButton.TranslateTo(cameraButtonEndPosition.X, cameraButtonEndPosition.Y, 300),
                //VideoButton.TranslateTo(videoButtonEndPosition.X, videoButtonEndPosition.Y, 200),
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

    public static class ScreenCoords
    {
        public static (double X, double Y) GetScreenCoordinates(this VisualElement view)
        {
            var screenCoordinateX = view.X;
            var screenCoordinateY = view.Y;

            var parent = (VisualElement)view.Parent;
            while (parent != null && parent.GetType().BaseType == typeof(View))
            {
                screenCoordinateX += parent.X;
                screenCoordinateY += parent.Y;
                parent = (VisualElement)parent.Parent;
            }
            return (screenCoordinateX, screenCoordinateY);
        }
    }
}