using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.ViewModels
{
    [Preserve(AllMembers = true)]
    public class BirthdayDayViewModel : BaseViewModel
    {
        public int Number { get; set; }
        public bool IsSelected { get; set; }
    }
}