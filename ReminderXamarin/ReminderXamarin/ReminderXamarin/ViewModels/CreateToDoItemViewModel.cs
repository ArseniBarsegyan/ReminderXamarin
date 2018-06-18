using System.Windows.Input;
using ReminderXamarin.Extensions;
using Xamarin.Forms;

namespace ReminderXamarin.ViewModels
{
    public class CreateToDoItemViewModel : BaseViewModel
    {
        public CreateToDoItemViewModel()
        {
            CreateToDoItemCommand = new Command<ToDoViewModel>(CreateToDoCommandExecute);
            DeleteToDoItemCommand = new Command<ToDoViewModel>(toDoModel => DeleteNoteCommandExecute(toDoModel));
        }
        
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