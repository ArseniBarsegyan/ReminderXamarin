using System;
using System.Threading.Tasks;

using ReminderXamarin.Core.Interfaces.Commanding;
using ReminderXamarin.Services.Navigation;
using ReminderXamarin.ViewModels.Base;

using Rm.Data.Data.Entities;
using Rm.Helpers;

using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class NewToDoViewModel : BaseNavigableViewModel
    {
        private bool _isEnabled;
        private DateTime? _dateTime;

        public NewToDoViewModel(INavigationService navigationService,
           ICommandResolver commandResolver)
           : base(navigationService)
        {
            CreateToDoCommand = commandResolver.AsyncCommand(CreateToDo, () => { return IsEnabled; });
            NavigateBackCommand = commandResolver.AsyncCommand(NavigateBack);
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is DateTime dateTime)
            {
                _dateTime = dateTime;
            }

            return base.InitializeAsync(navigationData);
        }

        public string Description { get; set; }

        public bool IsEnabled
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Description)
                    || Description.Length < 3)
                {
                    return false;
                }
                return true;
            }
            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public IAsyncCommand CreateToDoCommand { get; }
        public IAsyncCommand NavigateBackCommand { get; }

        private async Task CreateToDo()
        {
            var model = new ToDoModel
            {
                UserId = Settings.CurrentUserId,
                WhenHappens = _dateTime.Value,
                Description = Description,
                Status = ConstantsHelper.Active
            };
            App.ToDoRepository.Value.Save(model);
            MessagingCenter.Send(this, ConstantsHelper.ToDoItemCreated, model);
            await NavigateBack();
        }

        private async Task NavigateBack()
        {
            await NavigationService.NavigatePopupBackAsync();
        }
    }
}
