using ReminderXamarin.ViewModels;
using ReminderXamarin.ViewModels.Base;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.Views
{
    [Preserve(AllMembers = true)]
    public partial class MenuView : MasterDetailPage
    {
        private MenuViewModel ViewModel => BindingContext as MenuViewModel;
        
        public MenuView()
        {
            InitializeComponent();
            ViewModel.MasterViewModel = MenuMasterView.BindingContext as BaseNavigableViewModel;
            ViewModel.DetailViewModel = MenuDetailsView.BindingContext as BaseNavigableViewModel;
        }
    }
}