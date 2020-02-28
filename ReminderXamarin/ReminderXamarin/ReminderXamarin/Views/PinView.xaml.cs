using ReminderXamarin.Extensions;
using System;
using System.Threading.Tasks;

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
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await AnimateButtons();
        }

        private async Task AnimateButtons()
        {
            Button1.Scale = 0;
            Button2.Scale = 0;
            Button3.Scale = 0;
            Button4.Scale = 0;
            Button5.Scale = 0;
            Button6.Scale = 0;
            Button7.Scale = 0;
            Button8.Scale = 0;
            Button9.Scale = 0;
            Button0.Scale = 0;
            ButtonX.Scale = 0;

            await Task.Delay(300);
            var tasks = new Task[]
            {
                Button1.ScaleTo(1, 300),
                Button2.ScaleTo(1, 300),
                Button3.ScaleTo(1, 300),
                Button4.ScaleTo(1, 300),
                Button5.ScaleTo(1, 300),
                Button6.ScaleTo(1, 300),
                Button7.ScaleTo(1, 300),
                Button8.ScaleTo(1, 300),
                Button9.ScaleTo(1, 300),
                Button0.ScaleTo(1, 300),
                ButtonX.ScaleTo(1, 300)
            };
            await Task.WhenAll(tasks);
        }

        private async void Button_OnPressed(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                await button.ScaleTo(1.1f);
                //await button.ColorTo(Color.Transparent, Color.White, c => button.BackgroundColor = c, 15);
                //await button.ColorTo(Color.White, Color.FromHex("#323232"), c => button.TextColor = c, 15);
                //await button.ColorTo(Color.White, Color.Transparent, c => button.BackgroundColor = c, 15);
                //await button.ColorTo(Color.FromHex("#323232"), Color.White, c => button.TextColor = c, 15);
                await button.ScaleTo(1f);
            }
        }
    }
}
