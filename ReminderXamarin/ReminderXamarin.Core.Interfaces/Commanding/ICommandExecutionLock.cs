using System.Threading.Tasks;

namespace ReminderXamarin.Core.Interfaces.Commanding
{
    public interface ICommandExecutionLock
    {
        bool IsLocked { get; }
        bool TryLockExecution();
        Task<bool> FreeExecutionLock();
    }
}
