using ReminderXamarin.ViewModels;

using Rm.Helpers;

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
        }

        private void MenuList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is MasterPageItem item)
            {
                if (BindingContext is MenuMasterViewModel viewModel)
                {
                    MessagingCenter.Send(viewModel, ConstantsHelper.DetailPageChanged, item.Index);
                }
            }
            MenuList.SelectedItem = null;
        }
    }
}