using System;
using System.Linq;
using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateNotePage : ContentPage
    {
        /// <summary>
        /// Indicates if user tapped on "Save" button.
        /// </summary>
        private bool _saveClicked;

        public CreateNotePage()
        {
            InitializeComponent();
        }

        // Create note with photos and save them to SQLite DB
        private async void Save_OnClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DescriptionEditor.Text))
            {
                await DisplayAlert(ConstantHelper.Warning, ConstantHelper.NoteTextIsEmptyMessage, ConstantHelper.Ok);
                return;
            }

            if (!ViewModel.Photos.Any())
            {
                ViewModel.Photos.Add(new PhotoViewModel());
            }

            if (!_saveClicked)
            {
                DateTime currentDateTime = DateTime.Now;
                
                ViewModel.CreateNoteCommand.Execute(new NoteViewModel
                {
                    CreationDate = currentDateTime,
                    EditDate = currentDateTime,
                    Description = DescriptionEditor.Text,
                    Photos = ViewModel.Photos
                });
            }
            _saveClicked = true;
            await Navigation.PopAsync();
        }

        private void ViewModel_OnPhotoAdded(object sender, EventArgs e)
        {
            ImageGallery.Render();
        }
    }
}