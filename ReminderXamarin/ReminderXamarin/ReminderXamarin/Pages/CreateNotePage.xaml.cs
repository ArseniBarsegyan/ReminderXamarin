using System;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateNotePage : ContentPage
    {
        public CreateNotePage()
        {
            InitializeComponent();
        }

        // Create note with photos and save them to SQLite DB
        private async void Save_OnClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DescriptionEditor.Text))
            {
                await Navigation.PopAsync();
                return;
            }

            DateTime currentDateTime = DateTime.Now;

            ViewModel.CreateNoteCommand.Execute(new NoteViewModel
            {
                CreationDate = currentDateTime,
                EditDate = currentDateTime,
                Description = DescriptionEditor.Text,
                Photos = ViewModel.Photos
            });

            await Navigation.PopAsync();
        }

        private void ViewModel_OnPhotoAdded(object sender, EventArgs e)
        {
            ImageGallery.IsVisible = true;
            ImageGallery.Render();
        }
    }
}