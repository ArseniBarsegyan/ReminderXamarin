using ReminderXamarin.ViewModels;
using ReminderXamarin.ViewModels.Base;

using Xamarin.Forms;

namespace ReminderXamarin.Views
{
    public partial class MenuView : MasterDetailPage
    {
        public MenuView()
        {
            InitializeComponent();
            if (BindingContext is MenuViewModel viewModel)
            {
                viewModel.MasterViewModel = MenuMasterView.BindingContext as BaseViewModel;
                viewModel.DetailViewModel = MenuDetailsView.BindingContext as BaseViewModel;
            }
        }
    }
}