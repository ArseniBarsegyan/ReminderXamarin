using System.ComponentModel;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.ViewModels.Base
{
    [Preserve(AllMembers = true)]
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
