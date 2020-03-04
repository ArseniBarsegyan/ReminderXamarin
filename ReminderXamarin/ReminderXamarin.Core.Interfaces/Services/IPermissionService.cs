using System.Threading.Tasks;

namespace ReminderXamarin.Core.Interfaces
{
    public interface IPermissionService
    {
        Task<bool> AskPermission();
    }
}