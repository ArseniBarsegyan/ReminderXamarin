using ReminderXamarin.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.Views
{
    [Preserve(AllMembers = true)]
    public partial class UserProfileView : ContentPage
    {
        private UserProfileViewModel ViewModel => BindingContext as UserProfileViewModel;

        public UserProfileView()
        {
            InitializeComponent();
        }
    }
}