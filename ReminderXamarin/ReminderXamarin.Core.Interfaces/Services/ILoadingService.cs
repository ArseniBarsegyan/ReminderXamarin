namespace ReminderXamarin.Core.Interfaces
{
    public interface ILoadingService
    {
        void ShowLoading(string message = null);        
        void HideLoading();
    }
}