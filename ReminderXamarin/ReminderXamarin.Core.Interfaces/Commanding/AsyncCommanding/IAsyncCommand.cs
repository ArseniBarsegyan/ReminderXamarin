using System.Threading.Tasks;
using System.Windows.Input;

namespace ReminderXamarin.Core.Interfaces.Commanding.AsyncCommanding
{
    public interface IAsyncCommand<T> : ICommand
    {
        Task ExecuteAsync(T parameter);
    }
}
