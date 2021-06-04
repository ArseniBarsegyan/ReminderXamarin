using System.Linq;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.Views
{
    [Preserve(AllMembers = true)]
    public partial class BirthdaysView : ContentPage
    {
        private BirthdaysViewModel ViewModel => BindingContext as BirthdaysViewModel;
        
        public BirthdaysView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnAppearing();
        }

        private async void BirthdaysCollectionOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var model = e.CurrentSelection.FirstOrDefault() as BirthdayViewModel;
            BirthdaysCollection.SelectedItem = null;
            if (model != null)
            {
                await ViewModel.NavigateToEditBirthdayCommand.ExecuteAsync(model.Id);
            }
        }
    }
}