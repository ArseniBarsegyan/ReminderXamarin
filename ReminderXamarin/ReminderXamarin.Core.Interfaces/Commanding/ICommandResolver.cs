using ReminderXamarin.Core.Interfaces.Commanding.AsyncCommanding;

using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReminderXamarin.Core.Interfaces.Commanding
{
    public interface ICommandResolver
    {
        bool IsLocked { get; }
        IAsyncCommand AsyncCommand(Func<Task> execute, Func<bool> canExecute = null);
        IAsyncCommand<TParam> AsyncCommand<TParam>(Func<TParam, Task> execute, Func<object, bool> canExecute = null);
        ICommand Command(Action execute, Func<bool> canExecute = null);
        ICommand Command<TParam>(Action<TParam> execute, Func<object, bool> canExecute = null);
        void ForceRelease();
    }
}
