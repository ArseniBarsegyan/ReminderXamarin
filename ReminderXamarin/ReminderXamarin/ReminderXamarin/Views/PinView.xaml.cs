using System;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ReminderXamarin.Views
{
    public partial class PinView : ContentPage
    {
        public PinView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
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
                Button1.ScaleTo(1, 200),
                Button2.ScaleTo(1, 200),
                Button3.ScaleTo(1, 200),
                Button4.ScaleTo(1, 200),
                Button5.ScaleTo(1, 200),
                Button6.ScaleTo(1, 200),
                Button7.ScaleTo(1, 200),
                Button8.ScaleTo(1, 200),
                Button9.ScaleTo(1, 200),
                Button0.ScaleTo(1, 200),
                ButtonX.ScaleTo(1, 200)
            };
            await Task.WhenAll(tasks);
        }

        private async void Button_OnPressed(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                await button.ScaleTo(1.15f, 150, Easing.Linear);
                await button.ScaleTo(1f, 150, Easing.Linear);
            }
        }
    }
}
