using System;
using System.Reactive.Linq;
using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AchievementDetailView : ContentPage
    {
        private readonly AchievementViewModel _viewModel;

        public AchievementDetailView(AchievementViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _viewModel = viewModel;

            Observable.FromEventPattern(x => SaveNoteButton.Clicked += x, x => SaveNoteButton.Clicked -= x)
                .Subscribe(async _ =>
                {
                    await Navigation.PushModalAsync(new AchievementNoteCreateView(_viewModel));
                });

            Observable.FromEventPattern<SelectedItemChangedEventArgs>(x => AchievementNotes.ItemSelected += x,
                    x => AchievementNotes.ItemSelected -= x)
                .Subscribe(async item =>
                {
                    if (item.EventArgs.SelectedItem is AchievementNoteViewModel achievementNoteViewModel)
                    {
                        await Navigation.PushAsync(new AchievementNoteEditView(_viewModel, achievementNoteViewModel));
                    }
                    AchievementNotes.SelectedItem = null;
                });

            Observable.FromEventPattern(x => DeleteAchievementLink.Clicked += x,
                    x => DeleteAchievementLink.Clicked -= x)
                .Subscribe(async _ =>
                {
                    bool result = await DisplayAlert
                    (ConstantsHelper.Warning, ConstantsHelper.AchievementDeleteMessage, ConstantsHelper.Ok,
                        ConstantsHelper.Cancel);

                    if (result)
                    {
                        _viewModel.DeleteAchievementCommand.Execute(null);
                        await Navigation.PopAsync();
                    }
                });
        }

        protected override async void OnAppearing()
        {
            await _viewModel.OnAppearing();
            base.OnAppearing();
        }

        private async void DeleteAchievementNote_OnClicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert
                (ConstantsHelper.Warning, ConstantsHelper.AchievementNoteDeleteMessage, ConstantsHelper.Ok, ConstantsHelper.Cancel);
            if (result)
            {
                var menuItem = sender as MenuItem;
                var achievementNoteViewModel = menuItem?.CommandParameter as AchievementNoteViewModel;
                _viewModel.DeleteAchievementNoteCommand.Execute(achievementNoteViewModel);
                await _viewModel.OnAppearing();
            }
        }
    }
}