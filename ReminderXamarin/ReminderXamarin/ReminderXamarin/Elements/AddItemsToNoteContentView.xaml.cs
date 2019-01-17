using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Elements
{
    /// <inheritdoc />
    /// <summary>
    /// Contains frame with buttons "Take Photo", "Pick Photo", "Close".
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddItemsToNoteContentView : ContentView
    {
        public AddItemsToNoteContentView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invokes when button TakePhoto is clicked.
        /// </summary>
        public event EventHandler TakePhotoButtonClicked;
        /// <summary>
        /// Invokes when button PickPhoto is clicked.
        /// </summary>
        public event EventHandler PickPhotoButtonClicked;

        /// <summary>
        /// Invokes when button TakeVideo is clicked.
        /// </summary>
        public event EventHandler TakeVideoButtonClicked;

        /// <summary>
        /// Invokes when button PickVideo is clicked.
        /// </summary>
        public event EventHandler PickVideoButtonClicked;

        private void CancelButton_OnClicked(object sender, EventArgs e)
        {
            IsVisible = false;
        }

        private void PickPhotoButton_OnClicked(object sender, EventArgs e)
        {
            PickPhotoButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void TakePhotoButton_OnClicked(object sender, EventArgs e)
        {
            TakePhotoButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void TakeVideoButton_OnClicked(object sender, EventArgs e)
        {
            TakeVideoButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void PickVideoButton_OnClicked(object sender, EventArgs e)
        {
            PickVideoButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}