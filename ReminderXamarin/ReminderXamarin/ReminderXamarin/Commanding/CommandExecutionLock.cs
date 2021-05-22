using ReminderXamarin.Core.Interfaces.Commanding;

using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.Commanding
{
    public class CommandExecutionLock : ICommandExecutionLock
    {
        /// <summary>
        /// This interval is necessary to avoid multi tapping command from the user
        /// It can happen when user clicks simultaneously on several buttons on the screen
        /// </summary>
        public static int CommandExecutionInterval = 50;

        private readonly object _lockObject;

        [Preserve]
        public CommandExecutionLock()
        {
            _lockObject = new object();
            IsLockedImplementationNonBlocked = false;
        }

        private bool IsLockedImplementationNonBlocked { get; set; }

        public bool IsLocked
        {
            get
            {
                lock (_lockObject)
                {
                    return IsLockedImplementationNonBlocked;
                }
            }
        }

        public bool TryLockExecution()
        {
            if (IsLockedImplementationNonBlocked)
            {
                return false;
            }

            lock (_lockObject)
            {
                if (IsLockedImplementationNonBlocked)
                {
                    return false;
                }
                return IsLockedImplementationNonBlocked = true;
            }
        }

        public async Task<bool> FreeExecutionLock()
        {
            await Task.Delay(CommandExecutionInterval);
            if (!IsLockedImplementationNonBlocked)
            {
                return false;
            }

            lock (_lockObject)
            {
                if (!IsLockedImplementationNonBlocked)
                {
                    return false;
                }

                IsLockedImplementationNonBlocked = false;
                return true;
            }
        }
    }
}
