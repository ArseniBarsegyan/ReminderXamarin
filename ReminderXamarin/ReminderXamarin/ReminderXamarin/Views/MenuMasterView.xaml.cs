using System.Linq;
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

        private async void MenuCollectionViewOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is MasterPageItem item)
            {
                await ViewModel.ChangeDetailsPageCommand.ExecuteAsync(item.Index);
            }
        }
    }
}