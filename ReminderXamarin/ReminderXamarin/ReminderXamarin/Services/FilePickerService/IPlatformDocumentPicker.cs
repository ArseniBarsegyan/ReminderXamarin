using System.Threading.Tasks;

using Xamarin.Forms;

namespace ReminderXamarin.Services.FilePickerService
{
    public interface IPlatformDocumentPicker
    {
        Task<PlatformDocument> DisplayImportAsync(Page page);
    }
}