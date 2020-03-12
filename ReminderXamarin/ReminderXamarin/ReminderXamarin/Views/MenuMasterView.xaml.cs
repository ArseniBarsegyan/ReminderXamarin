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

        private async void MenuList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            MenuList.SelectedItem = null;
            if (e.SelectedItem is MasterPageItem item)
            {
                if (BindingContext is MenuMasterViewModel viewModel)
                {
                    await viewModel.ChangeDetailsPageCommand.ExecuteAsync(item.Index);
                }
            }            
        }
    }
}