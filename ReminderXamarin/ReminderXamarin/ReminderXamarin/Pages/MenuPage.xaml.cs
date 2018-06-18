using System;
using System.Linq;
using ReminderXamarin.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : MasterDetailPage, IDisposable
    {
        public MenuPage()
        {
            InitializeComponent();
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
            MessagingCenter.Unsubscribe<NotesPage, MenuPageIndex>(this, ConstantHelper.DetailPageChanged);
        }
    }
}