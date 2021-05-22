using ReminderXamarin.ViewModels;

using Rm.Helpers;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.Views
{
    [Preserve(AllMembers = true)]
    public partial class MenuMasterView : ContentPage
    {
        private MenuMasterViewModel ViewModel => BindingContext as MenuMasterViewModel;
        
        public MenuMasterView()
        {
            InitializeComponent();
        }

        private async void MenuList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            MenuList.SelectedItem = null;
            if (e.SelectedItem is MasterPageItem item)
            {
                await ViewModel.ChangeDetailsPageCommand.ExecuteAsync(item.Index);
            }            
        }
    }
}