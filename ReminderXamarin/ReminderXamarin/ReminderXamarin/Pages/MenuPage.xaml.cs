using System;
using System.IO;
using System.Linq;
using ReminderXamarin.Helpers;
using ReminderXamarin.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : MasterDetailPage, IDisposable
    {
        private UserModel _appUser;
        private string _userName;

        public MenuPage(string userName)
        {
            InitializeComponent();
            var user = App.UserRepository.GetAll().FirstOrDefault(x => x.UserName == userName);
            if (user != null)
            {
                _appUser = user;
            }
            _userName = userName;
            BindingContext = _appUser;

            NavigationPage.SetHasNavigationBar(this, false);
            var pages = MenuHelper.GetMenu().Where(x => x.IsDisplayed).ToList();
            MenuList.ItemsSource = pages;

            MessagingCenter.Subscribe<NotesPage, MenuPageIndex>(this, ConstantHelper.DetailPageChanged, (sender, pageIndex) =>
            {
                switch (pageIndex)
                {
                    case MenuPageIndex.NotesPage:
                        Navigation.PushAsync(new NotesPage());
                        break;
                    case MenuPageIndex.ToDoPage:
                        Navigation.PushAsync(new ToDoPage());
                        break;
                    case MenuPageIndex.BirthdaysPage:
                        Navigation.PushAsync(new BirthdaysPage());
                        break;
                    case MenuPageIndex.AchievementsPage:
                        Navigation.PushAsync(new AchievementsPage());
                        break;
                    default:
                        break;
                }
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            if (_appUser != null)
            {
                byte[] fileContent = _appUser.ImageContent;
                UserProfilePhoto.Source = ImageSource.FromStream(() => new MemoryStream(fileContent));
            }

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

            MessagingCenter.Subscribe<UserProfilePage>(this, ConstantHelper.ProfileUpdated, userProfilePage =>
            {
                _appUser = App.UserRepository.GetAll().FirstOrDefault(x => x.UserName == _userName);
                BindingContext = _appUser;
            });
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
            MessagingCenter.Unsubscribe<NotesPage, MenuPageIndex>(this, ConstantHelper.DetailPageChanged);
            MessagingCenter.Unsubscribe<UserProfilePage>(this, ConstantHelper.ProfileUpdated);
        }

        private void UserProfile_OnTapped(object sender, EventArgs e)
        {
            var userProfilePage = new UserProfilePage(_appUser.UserName);
            NavigateTo(userProfilePage);
        }
    }
}