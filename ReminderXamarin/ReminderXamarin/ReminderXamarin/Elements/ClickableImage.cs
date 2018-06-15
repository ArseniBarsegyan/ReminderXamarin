using System.Linq;
using ReminderXamarin.Views;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace ReminderXamarin.Elements
{
    /// <summary>
    /// Image with click handler. When user tap on this image he will see image in full size.
    /// <para>
    /// Required custom renderer creation.
    /// </para>
    /// </summary>
    public class ClickableImage : Image
    {
        /// <summary>
        /// Push popup page with full-size image to navigation stack.
        /// </summary>
        public void ShowFullSizeImage()
        {
            var fullSizeView = new FullSizeImageView(this);
            if (!Application.Current.MainPage.Navigation.NavigationStack.Contains(fullSizeView))
            {
                Application.Current.MainPage.Navigation.PushPopupAsync(fullSizeView);
            }
        }

        
        public static readonly BindableProperty FullSizeImageSourceProperty = 
            BindableProperty.Create(nameof(FullSizeImageSource), typeof(string), typeof(ClickableImage));

        /// <summary>
        /// High-resolution image source.
        /// </summary>
        /// 
        public string FullSizeImageSource
        {
            get => (string)GetValue(FullSizeImageSourceProperty);
            set => SetValue(FullSizeImageSourceProperty, value);
        }
    }
}