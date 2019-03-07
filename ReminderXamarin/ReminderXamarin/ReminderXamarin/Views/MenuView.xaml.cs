using ReminderXamarin.ViewModels;
using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuView : MasterDetailPage
    {
        public MenuView()
        {
            InitializeComponent();
            if (this.BindingContext is MenuViewModel viewModel)
            {
                viewModel.MasterViewModel = MenuMasterView.BindingContext as BaseViewModel;
                viewModel.DetailViewModel = MenuDetailsView.BindingContext as BaseViewModel;
            }
        }
    }
}