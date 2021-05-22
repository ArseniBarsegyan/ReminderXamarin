using System;
using System.Windows.Input;
using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.ViewModels
{
    [Preserve(AllMembers = true)]
    public class DayViewModel : BaseViewModel
    {
        public bool HasActiveToDo { get; set; }
        public bool HasCompletedToDo { get; set; }
        public bool Selected { get; set; }
        public DateTime CurrentDate { get; set; }
        public DayPosition DayPosition { get; set; }
        public ICommand DaySelectedCommand { get; set; }
        public ICommand DayUnselectedCommand { get; set; }
    }

    [Preserve(AllMembers = true)]
    public struct DayPosition
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
