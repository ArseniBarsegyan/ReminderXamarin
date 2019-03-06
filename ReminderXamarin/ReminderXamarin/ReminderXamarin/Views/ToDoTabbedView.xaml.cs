using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToDoTabbedView : TabbedPage
    {
        public ToDoTabbedView ()
        {
            InitializeComponent();
        }
    }
}