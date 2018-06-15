using System;
using ReminderXamarin.Elements;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    /// <summary>
    /// This view shows image in full size.
    /// as popup
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FullSizeImageView : PopupPage
    {
        /// <summary>
        /// Create new instance of <see cref="FullSizeImageView"/>.
        /// </summary>
        /// <param name="clickableImage">Clickable image</param>
        public FullSizeImageView(ClickableImage clickableImage)
        {
            InitializeComponent();
            BindingContext = clickableImage;
            CloseWhenBackgroundIsClicked = true;
        }

        // Close current popup page if user tap outside of the image.
        private async void Background_OnClick(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

        // Calling when user tap on image.
        private void Image_OnTapped(object sender, EventArgs e)
        {
        }
    }
}