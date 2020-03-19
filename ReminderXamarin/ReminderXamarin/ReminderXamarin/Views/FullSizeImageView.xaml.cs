using System;

using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FullSizeImageView : PopupPage
    {
        public FullSizeImageView(ImageSource imageSource)
        {
            InitializeComponent();
            Image.Source = imageSource;
        }

        private async void Background_OnClick(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
    }
}