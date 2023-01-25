using System.Threading.Tasks;

namespace ReminderXamarin.Services.FilePickerService
{
    public interface IPlatformDocumentPicker
    {
        Task<PlatformDocument> DisplayImageImportAsync();
        Task<PlatformDocument> DisplayTextImportAsync();
    }
}