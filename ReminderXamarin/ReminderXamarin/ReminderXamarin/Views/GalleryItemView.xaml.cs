using ReminderXamarin.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    /// <summary>
    /// This view shows image in full size.
    /// as popup
    /// </summary>
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