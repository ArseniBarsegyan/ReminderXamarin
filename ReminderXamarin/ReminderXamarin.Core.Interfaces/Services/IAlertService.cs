using System.Threading.Tasks;

namespace ReminderXamarin.Core.Interfaces
{
    public interface IAlertService
    {
        Task<bool> ShowYesNoAlert(string message, string yesButtonText, string noButtonText);
    }
}