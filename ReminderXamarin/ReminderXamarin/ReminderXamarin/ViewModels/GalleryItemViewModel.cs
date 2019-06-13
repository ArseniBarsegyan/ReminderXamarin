using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.ViewModels.Base;
using Rm.Helpers;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class GalleryItemViewModel : BaseViewModel
    {
        public GalleryItemViewModel()
        {
            DeleteCommand = new Command(async() => await Delete());
            NavigateBackCommand = new Command(async () => await NavigateBack());
        }
        
        public int Id { get; set; }

        public string ImagePath { get; set; }
        public string Thumbnail { get; set; }
        public bool IsVideo { get; set; }
        public string VideoPath { get; set; }

        public int NoteId { get; set; }

        public ICommand DeleteCommand { get; set; }
        public ICommand NavigateBackCommand { get; set; }

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
                MessagingCenter.Send(this, ConstantsHelper.ImageDeleted);
                await NavigateBack();
            }
        }

        private async Task NavigateBack()
        {
            await NavigationService.NavigatePopupBackAsync();
        }
    }
}