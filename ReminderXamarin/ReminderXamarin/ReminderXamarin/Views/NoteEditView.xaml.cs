using ReminderXamarin.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoteEditView : ContentPage
    {
        public NoteEditView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is NoteEditViewModel noteEditViewModel)
            {
                if (!noteEditViewModel.IsEditMode)
                {
                    ToolbarItems.Remove(DeleteOption);
                }
                noteEditViewModel.OnAppearing();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (BindingContext is NoteEditViewModel noteEditViewModel)
            {
                noteEditViewModel.OnDisappearing();
            }
        }
    }
}