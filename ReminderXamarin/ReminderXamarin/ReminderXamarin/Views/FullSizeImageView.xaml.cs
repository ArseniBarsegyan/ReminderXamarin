using System;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
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
        /// <param name="image">Image</param>
        public FullSizeImageView(Image image)
        {
            InitializeComponent();
            BindingContext = image;
            CloseWhenBackgroundIsClicked = true;
        }

        private void AddImageToLayout(Image image)
        {
            AbsoluteLayout.SetLayoutBounds(image, new Rectangle(0.5, 0.5, 300, 300));
            AbsoluteLayout.SetLayoutFlags(image, AbsoluteLayoutFlags.PositionProportional);
            ImageContainer.Children.Add(image);
        }

        // Close current popup page if user tap outside of the image.
        private async void Background_OnClick(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
    }
}