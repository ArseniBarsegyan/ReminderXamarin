using System;
using System.Linq;
using ReminderXamarin.Extensions;
using ReminderXamarin.Helpers;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuView : MasterDetailPage, IDisposable
    {
        private readonly UserProfileViewModel _appUser;

        public MenuView(string userName)
        {
            InitializeComponent();
            var user = App.UserRepository.GetAll().FirstOrDefault(x => x.UserName == userName);
            if (user != null)
            {
                _appUser = user.ToUserProfileViewModel();
                Settings.CurrentUserId = user.Id.ToString();
            }
            BindingContext = _appUser;

            NavigationPage.SetHasNavigationBar(this, false);
            var pages = MenuHelper.GetMenu().Where(x => x.IsDisplayed).ToList();
            MenuList.ItemsSource = pages;

            BackgroundImage.Source = ImageSource.FromResource(ConstantsHelper.SideMenuBackground);

            MessagingCenter.Subscribe<NotesView, MenuViewIndex>(this, ConstantsHelper.DetailPageChanged, (sender, pageIndex) =>
            {
                switch (pageIndex)
                {
                    case MenuViewIndex.NotesView:
                        Navigation.PushAsync(new NotesView());
                        break;
                    case MenuViewIndex.ToDoPage:
                        Navigation.PushAsync(new ToDoTabbedView());
                        break;
                    case MenuViewIndex.BirthdaysView:
                        Navigation.PushAsync(new BirthdaysView());
                        break;
                    case MenuViewIndex.AchievementsView:
                        Navigation.PushAsync(new AchievementsView());
                        break;
                    case MenuViewIndex.SettingsView:
                        Navigation.PushAsync(new SettingsView());
                        break;
                    default:
                        break;
                }
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Navigation.NavigationStack.Count > 0)
            {
                var pages = Navigation.NavigationStack;
                for (int i = 0; i < pages.Count; i++)
                {
                    if (pages[i] != this)
                    {
                        Navigation.RemovePage(pages[i]);
                    }
                }
            }
        }

        private void MenuList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is MasterPageItem item)
            {
                MenuList.SelectedItem = null;
                if (item.TargetType != typeof(string))
                {
                    var page = Activator.CreateInstance(item.TargetType) as Page;
                    NavigateTo(page);
                }
            }
        }

        public void NavigateTo(Page page)
        {
            Detail = new NavigationPage(page);
            IsPresented = false;
        }

        public void Dispose()
        {
            MessagingCenter.Unsubscribe<NotesView, MenuViewIndex>(this, ConstantsHelper.DetailPageChanged);
            MessagingCenter.Unsubscribe<UserProfileView>(this, ConstantsHelper.ProfileUpdated);
        }

        private void UserProfile_OnTapped(object sender, EventArgs e)
        {
            if (_appUser != null)
            {
                var userProfilePage = new UserProfileView(_appUser);
                NavigateTo(userProfilePage);
            }
        }

        private void Logout_OnTapped(object sender, EventArgs e)
        {
            bool.TryParse(Settings.UsePin, out var result);
            if (result)
            {
                // App.NavigationService.InitializeAsync<PinViewModel>();
                Application.Current.MainPage = new PinView();
            }
            else
            {
                // App.NavigationService.InitializeAsync<LoginViewModel>();
                Application.Current.MainPage = new LoginView();
            }
        }
    }
}