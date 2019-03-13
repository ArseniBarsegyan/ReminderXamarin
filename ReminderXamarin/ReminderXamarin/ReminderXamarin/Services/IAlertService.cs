using System.Threading.Tasks;

namespace ReminderXamarin.Services
{
    /// <summary>
    /// This service provides custom alerts.
    /// </summary>
    public interface IAlertService
    {
        Task<bool> ShowYesNoAlert(string message, string yesButtonText, string noButtonText);
    }
}