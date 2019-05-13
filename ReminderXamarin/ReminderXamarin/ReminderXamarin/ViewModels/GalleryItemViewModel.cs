using System.Threading.Tasks;
using System.Windows.Input;
using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class GalleryItemViewModel : BaseViewModel
    {
        public GalleryItemViewModel()
        {
            NavigateBackCommand = new Command(async () => await NavigateBack());
        }
        
        public int Id { get; set; }

        public string ImagePath { get; set; }
        public string Thumbnail { get; set; }
        public bool IsVideo { get; set; }
        public string VideoPath { get; set; }

        public int NoteId { get; set; }

        public ICommand NavigateBackCommand { get; set; }

        private async Task NavigateBack()
        {
            await NavigationService.NavigatePopupBackAsync();
        }
    }
}