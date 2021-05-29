using System;
using System.Windows.Input;
using PropertyChanged;
using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.ViewModels
{
    [Preserve(AllMembers = true)]
    public class DayViewModel : BaseViewModel
    {
        public bool HasActiveToDo { get; set; }
        public bool HasCompletedToDo { get; set; }
        
        [AlsoNotifyFor(nameof(SelectedColor))]
        public bool Selected { get; set; }
        public DateTime CurrentDate { get; set; }
        public DayPosition DayPosition { get; set; }
        public ICommand DaySelectedCommand { get; set; }
        public ICommand DayUnselectedCommand { get; set; }
        
        public Color SelectedColor
        {
            get
            {
                if (CurrentDate.DayOfWeek == DayOfWeek.Saturday)
                    return (Color)Application.Current.Resources["CalendarSaturdayText"];

                if (CurrentDate.DayOfWeek == DayOfWeek.Sunday)
                    return (Color)Application.Current.Resources["CalendarSundayText"];

                return (Color)Application.Current.Resources["TextCommon"];
            }
        }
    }

    [Preserve(AllMembers = true)]
    public struct DayPosition
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
