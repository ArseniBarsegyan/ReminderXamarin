using System;
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

        private void Button_OnPressed(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                button.BackgroundColor = Color.White;
                button.TextColor = Color.FromHex("#323232");

                Device.StartTimer(TimeSpan.FromSeconds(0.05), () =>
                {
                    button.BackgroundColor = Color.Transparent;
                    button.TextColor = Color.White;
                    return false;
                });
            }
        }
    }
}