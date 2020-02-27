using ReminderXamarin.ViewModels;

using Rg.Plugins.Popup.Pages;

using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GalleryItemView : PopupPage
    {
        public GalleryItemView(GalleryItemViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}