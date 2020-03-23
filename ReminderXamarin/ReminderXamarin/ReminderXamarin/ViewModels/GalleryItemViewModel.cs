using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

using Rm.Data.Data.Entities;
using Rm.Helpers;

using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class GalleryItemViewModel : BaseViewModel
    {
        private GalleryItemModel _model;

        public GalleryItemViewModel(INavigationService navigationService,
            ICommandResolver commandResolver)
            : base(navigationService)
        {
            DeleteCommand = commandResolver.AsyncCommand(Delete);
            NavigateBackCommand = commandResolver.AsyncCommand(NavigateBack);
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is GalleryItemModel model)
            {
                _model = model;
                ImagePath = model.ImagePath;
            }

            return base.InitializeAsync(navigationData);
        }

        public string ImagePath { get; set; }

        public ICommand DeleteCommand { get; }
        public ICommand NavigateBackCommand { get; }

        private async Task Delete()
        {
            var deleteQuestionMessage =
                Resmgr.Value.GetString(ConstantsHelper.DeleteGalleryItemQuestion, CultureInfo.CurrentCulture);
            var okLocalized = Resmgr.Value.GetString(ConstantsHelper.Ok, CultureInfo.CurrentCulture);
            var cancelLocalized = Resmgr.Value.GetString(ConstantsHelper.Cancel, CultureInfo.CurrentCulture);

            var result = await Acr.UserDialogs.UserDialogs.Instance.ConfirmAsync(deleteQuestionMessage, null, okLocalized,
                cancelLocalized);
            if (result)
            {
                MessagingCenter.Send(this, ConstantsHelper.ImageDeleted, _model.Id);
                await NavigateBack();
            }
        }

        private async Task NavigateBack()
        {
            await NavigationService.NavigatePopupBackAsync();
        }
    }
}