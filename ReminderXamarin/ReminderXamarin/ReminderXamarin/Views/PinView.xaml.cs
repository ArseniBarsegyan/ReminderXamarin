using System;
using ReminderXamarin.Extensions;
using Rm.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PinView : ContentPage
    {
        public PinView()
        {
            InitializeComponent();
            BackgroundImage.Source = ImageSource.FromResource(ConstantsHelper.BackgroundImageSource);
        }

        private async void Button_OnPressed(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                await button.ColorTo(Color.Transparent, Color.White, c => button.BackgroundColor = c, 25);
                await button.ColorTo(Color.White, Color.FromHex("#323232"), c => button.TextColor = c, 25);
                await button.ColorTo(Color.White, Color.Transparent, c => button.BackgroundColor = c, 25);
                await button.ColorTo(Color.FromHex("#323232"), Color.White, c => button.TextColor = c, 25);
            }
        }
    }
}
