using Rm.Helpers;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuMasterView : ContentPage
    {
        public MenuMasterView()
        {
            InitializeComponent();
            HeaderBackgroundImage.Source = ImageSource.FromResource(ConstantsHelper.SideMenuBackground);
        }

        private void MenuList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is MasterPageItem item)
            {
                if (this.BindingContext is MenuMasterViewModel viewModel)
                {
                    MessagingCenter.Send(viewModel, ConstantsHelper.DetailPageChanged, item.Index);
                }
            }
            MenuList.SelectedItem = null;
        }
    }
}