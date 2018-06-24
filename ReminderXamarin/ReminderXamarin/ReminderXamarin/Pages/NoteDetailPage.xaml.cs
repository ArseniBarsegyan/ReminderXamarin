using System;
using System.Linq;
using ReminderXamarin.Elements;
using ReminderXamarin.Extensions;
using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoteDetailPage : ContentPage
    {
        private readonly NoteViewModel _noteViewModel;
        private readonly ToolbarItem _confirmToolbarItem;

        public NoteDetailPage(NoteViewModel noteViewModel)
        {
            InitializeComponent();
            BindingContext = noteViewModel;
            _noteViewModel = noteViewModel;

            _confirmToolbarItem = new ToolbarItem { Icon = "confirm.png" };

            Title = $"{noteViewModel.EditDate:d}";
            DescriptionEditor.Text = noteViewModel.Description;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _confirmToolbarItem.Clicked += Confirm_OnClicked;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _confirmToolbarItem.Clicked -= Confirm_OnClicked;
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

            if (ToolbarItems.Contains(_confirmToolbarItem))
            {
                ToolbarItems.RemoveAt(1);
            }
        }

        private void DescriptionEditor_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!ToolbarItems.Contains(_confirmToolbarItem))
            {
                ToolbarItems.Add(_confirmToolbarItem);
            }
        }

        private async void HorizontalImageGallery_OnItemTapped(object sender, EventArgs e)
        {
            if (sender is Image tappedImage)
            {
                FileImageSource fileImageSource = (FileImageSource)tappedImage.Source;
                string filePath = fileImageSource.File;
                _noteViewModel.SelectedPhoto = _noteViewModel.Photos.FirstOrDefault(x => x.ResizedPath == filePath);
                var images = _noteViewModel.Photos.ToImages();
                var currentImage = _noteViewModel.SelectedPhoto.ToImage();

                await Navigation.PushPopupAsync(new FullSizeImageGallery(images, currentImage));
            }
        }
    }
}