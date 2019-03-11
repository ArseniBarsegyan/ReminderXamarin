using System.Threading.Tasks;
using ReminderXamarin.ViewModels.Base;

namespace ReminderXamarin.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        public BaseViewModel MasterViewModel { get; set; }
        public BaseViewModel DetailViewModel { get; set; }

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