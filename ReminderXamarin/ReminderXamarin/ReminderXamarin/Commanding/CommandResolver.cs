using ReminderXamarin.Commanding.AsyncCommanding;
using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Core.Interfaces.Commanding.AsyncCommanding;

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace ReminderXamarin.Commanding
{
    public class CommandResolver : ICommandResolver
    {
        private readonly ICommandExecutionLock _commandExecutionLock;
        private long _lockIndex;

        public CommandResolver(ICommandExecutionLock commandExecutionLock)
        {
            _commandExecutionLock = commandExecutionLock;
        }

        public bool IsLocked => _commandExecutionLock.IsLocked;

        public IAsyncCommand AsyncCommandWithoutLock(Func<Task> execute, Func<bool> canExecute = null)
        {
            return new AsyncCommand(execute, canExecute);
        }

        public IAsyncCommand<TParam> AsyncCommandWithoutLock<TParam>(Func<TParam, Task> execute, Func<object, bool> canExecute = null)
        {
            return new AsyncCommand<TParam>(execute, par => CanExecute(par, canExecute));
        }

        public IAsyncCommand AsyncCommand(
            Func<Task> execute, Func<bool> canExecute = null)
        {
            async Task Func()
            {
                if (IsLocked)
                {
                    return;
                }

                long currentLockIndex = 0;
                try
                {
                    if (!_commandExecutionLock.TryLockExecution())
                    {
                        return;
                    }

                    currentLockIndex = Interlocked.Increment(ref _lockIndex);
                    try
                    {
                        await execute();
                    }
                    catch (Exception e)
                    {
                        if (!HandleException(e))
                        {
                            throw;
                        }
                    }
                }
                finally
                {
                    if (Interlocked.Read(ref _lockIndex) == currentLockIndex)
                    {
                        _commandExecutionLock.FreeExecutionLock();
                    }
                }
            }

            return new AsyncCommand(Func, canExecute);
        }

        public IAsyncCommand<TParam> AsyncCommand<TParam>(
            Func<TParam, Task> execute, Func<object, bool> canExecute = null)
        {
            async Task Func(TParam param)
            {
                if (IsLocked)
                {
                    return;
                }

                long currentLockIndex = 0;
                try
                {
                    if (!_commandExecutionLock.TryLockExecution())
                    {
                        return;
                    }

                    currentLockIndex = Interlocked.Increment(ref _lockIndex);
                    try
                    {
                        await execute(param);
                    }
                    catch (Exception e)
                    {
                        if (!HandleException(e))
                        {
                            throw;
                        }
                    }
                }
                finally
                {
                    if (Interlocked.Read(ref _lockIndex) == currentLockIndex)
                    {
                        _commandExecutionLock.FreeExecutionLock();
                    }
                }
            }

            return new AsyncCommand<TParam>(Func, par => CanExecute(par, canExecute));
        }

        public void ForceRelease()
        {
            Interlocked.Increment(ref _lockIndex);
            _commandExecutionLock.FreeExecutionLock();
        }

        public ICommand Command(Action execute, Func<bool> canExecute = null)
        {
            return Command<object>(o => execute(), o => CanExecute(canExecute));
        }

        public ICommand CommandWithoutLock(Action execute, Func<bool> canExecute = null)
        {
            return CommandWithoutLock<object>(o => execute(), o => CanExecute(canExecute));
        }

        public ICommand CommandWithoutLock<TParam>(Action<TParam> execute, Func<object, bool> canExecute = null)
        {
            void Action(TParam p)
            {
                try
                {
                    execute(p);
                }
                finally
                {
                    _commandExecutionLock.FreeExecutionLock();
                }
            }

            return new Command<TParam>(Action, par => CanExecute(par, canExecute));
        }

        public ICommand Command<TParam>(Action<TParam> execute, Func<object, bool> canExecute = null)
        {
            void Action(TParam p)
            {
                if (_commandExecutionLock.IsLocked)
                {
                    return;
                }

                try
                {
                    if (!_commandExecutionLock.TryLockExecution())
                    {
                        return;
                    }

                    execute(p);
                }
                finally
                {
                    _commandExecutionLock.FreeExecutionLock();
                }
            }

            return new Command<TParam>(Action, par => CanExecute(par, canExecute));
        }

        protected virtual bool HandleException(Exception e)
        {
            return false;
        }

        private static bool CanExecute<TParam>(TParam par, Func<object, bool> canExecute = null)
        {
            return canExecute == null || canExecute(par);
        }

        private static bool CanExecute(Func<bool> canExecute = null)
        {
            return canExecute == null || canExecute();
        }

        IAsyncCommand<TParam> ICommandResolver.AsyncCommand<TParam>(Func<TParam, Task> execute, Func<object, bool> canExecute)
        {
            async Task Func(TParam param)
            {
                if (IsLocked)
                {
                    return;
                }

                long currentLockIndex = 0;
                try
                {
                    if (!_commandExecutionLock.TryLockExecution())
                    {
                        return;
                    }

                    currentLockIndex = Interlocked.Increment(ref _lockIndex);
                    try
                    {
                        await execute(param);
                    }
                    catch (Exception e)
                    {
                        if (!HandleException(e))
                        {
                            throw;
                        }
                    }
                }
                finally
                {
                    if (Interlocked.Read(ref _lockIndex) == currentLockIndex)
                    {
                        _commandExecutionLock.FreeExecutionLock();
                    }
                }
            }

            return new AsyncCommand<TParam>(Func, par => CanExecute(par, canExecute));
        }
    }
}
