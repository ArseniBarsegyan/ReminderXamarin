using System.Threading.Tasks;

using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.ViewModels
{
    [Preserve(AllMembers = true)]
    public class MenuViewModel : BaseNavigableViewModel
    {
        public BaseNavigableViewModel MasterViewModel { get; set; }
        public BaseNavigableViewModel DetailViewModel { get; set; }

        public MenuViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }

        public override async Task InitializeAsync(object navigationData)
        {
            await base.InitializeAsync(navigationData);

            if (MasterViewModel != null)
            {
                await MasterViewModel.InitializeAsync(navigationData);
            }

            if (DetailViewModel != null)
            {
                await DetailViewModel.InitializeAsync(navigationData);
            }
        }
    }
}