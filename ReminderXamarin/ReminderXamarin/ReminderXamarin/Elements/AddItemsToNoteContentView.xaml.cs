using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Elements
{
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
    }
}