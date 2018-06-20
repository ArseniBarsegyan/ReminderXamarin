using System.Collections.ObjectModel;
using System.Windows.Input;
using ReminderXamarin.Extensions;
using ReminderXamarin.Models;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class CreateToDoItemViewModel : BaseViewModel
    {
        public CreateToDoItemViewModel()
        {
            AvailablePriorities = new ObservableCollection<ToDoPriority>
            {
                ToDoPriority.High,
                ToDoPriority.Medium,
                ToDoPriority.Low
            };

            CreateToDoItemCommand = new Command<ToDoViewModel>(CreateToDoCommandExecute);
            DeleteToDoItemCommand = new Command<ToDoViewModel>(toDoModel => DeleteNoteCommandExecute(toDoModel));
        }

        public ObservableCollection<ToDoPriority> AvailablePriorities { get; set; }
        public ToDoPriority Priority { get; set; } = ToDoPriority.Medium;
        public ICommand CreateToDoItemCommand { get; set; }
        public ICommand DeleteToDoItemCommand { get; set; }

        private void CreateToDoCommandExecute(ToDoViewModel viewModel)
        {
            App.ToDoRepository.Save(viewModel.ToToDoModel());
        }
        private int DeleteNoteCommandExecute(ToDoViewModel viewModel)
        {
            return App.ToDoRepository.DeleteModel(viewModel.ToToDoModel());
        }
    }
}