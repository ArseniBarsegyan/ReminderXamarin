using System.Threading.Tasks;
using System.Windows.Input;

namespace ReminderXamarin.Core.Interfaces.Commanding
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync();
        bool CanExecute();
    }
}
