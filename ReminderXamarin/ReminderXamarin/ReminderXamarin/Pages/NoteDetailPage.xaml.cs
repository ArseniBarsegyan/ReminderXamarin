using System;
using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoteDetailPage : ContentPage
    {
        private readonly NoteViewModel _noteViewModel;

        public NoteDetailPage(NoteViewModel noteViewModel)
        {
            InitializeComponent();
            BindingContext = noteViewModel;
            _noteViewModel = noteViewModel;
            Title = $"{noteViewModel.EditDate:d}";
            DescriptionEditor.Text = noteViewModel.Description;
        }

        private async void Delete_OnClicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert
                (ConstantHelper.Warning, ConstantHelper.NoteDeleteMessage, ConstantHelper.Ok, ConstantHelper.Cancel);
            if (result)
            {
                _noteViewModel.DeleteNoteCommand.Execute(_noteViewModel);
                await Navigation.PopAsync();
            }
        }

        private void Confirm_OnClicked(object sender, EventArgs e)
        {
            _noteViewModel.Description = DescriptionEditor.Text;
            _noteViewModel.UpdateNoteCommand.Execute(_noteViewModel);
        }
    }
}