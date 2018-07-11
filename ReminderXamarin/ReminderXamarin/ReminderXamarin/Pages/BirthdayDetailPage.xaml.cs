using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BirthdayDetailPage : ContentPage
    {
        public BirthdayDetailPage(BirthdayViewModel viewModel)
        {
            InitializeComponent();
        }
    }
}