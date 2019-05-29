using System;
using ReminderXamarin.Services.FilePickerService;
using ReminderXamarin.ViewModels;
using Rm.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementEditView : ContentPage
    {
        private readonly ToolbarItem _confirmToolbarItem;
        private static readonly IPlatformDocumentPicker DocumentPicker = DependencyService.Get<IPlatformDocumentPicker>();

        public AchievementEditView()
        {
            InitializeComponent();
            _confirmToolbarItem = new ToolbarItem { Order = ToolbarItemOrder.Primary, IconImageSource = ConstantsHelper.ConfirmIcon };
            _confirmToolbarItem.Clicked += Confirm_OnClicked;
            ToolbarItems.Add(_confirmToolbarItem);
            BackgroundImage.Source = ImageSource.FromResource(ConstantsHelper.BackgroundImageSource);
        }

        private void Confirm_OnClicked(object sender, EventArgs e)
        {
            if (BindingContext is AchievementEditViewModel editViewModel)
            {
                editViewModel.Description = DescriptionEditor.Text;
                editViewModel.Title = TitleEntry.Text;
                editViewModel.SaveAchievementCommand.Execute(null);

                if (ToolbarItems.Contains(_confirmToolbarItem))
                {
                    ToolbarItems.Remove(_confirmToolbarItem);
                }
            }
        }

        private void DescriptionEditor_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!ToolbarItems.Contains(_confirmToolbarItem))
            {
                ToolbarItems.Add(_confirmToolbarItem);
            }
        }

        private async void AchievementImage_OnTapped(object sender, EventArgs e)
        {
            var document = await DocumentPicker.DisplayImportAsync(this);
            if (document == null)
            {
                return;
            }
            if (BindingContext is AchievementEditViewModel viewModel)
            {
                viewModel.ChangeImageCommand.Execute(document);
            }
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            AchievementStepsListView.SelectedItem = null;
        }
    }
}