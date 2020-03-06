using ReminderXamarin.ViewModels.Base;
using ReminderXamarin.Views;

using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;

using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ReminderXamarin.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        public BaseViewModel PreviousPageViewModel
        {
            get
            {
                if (Application.Current.MainPage is NavigationPage mainPage &&
                    mainPage.Navigation.NavigationStack.Count > 1)
                {
                    var viewModel = mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2].BindingContext;
                    return viewModel as BaseViewModel;
                }
                return null;
            }
        }

        public Task InitializeAsync<TViewModel>() where TViewModel : BaseViewModel
        {
            return NavigateToAsync<TViewModel>();
        }

        public async Task NavigateToRootAsync()
        {
            if (Application.Current.MainPage is NavigationPage mainPage)
            {
                var lastPage = mainPage.Navigation.NavigationStack.Last();
                await lastPage.Navigation.PopToRootAsync();
            }
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : BaseViewModel
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public Task NavigateToPopupAsync<TViewModel>(object parameter) where TViewModel : BaseViewModel
        {
            return InternalNavigateToPopupAsync(typeof(TViewModel), parameter);
        }

        private async Task InternalNavigateToPopupAsync(Type viewModelType, object parameter)
        {
            PopupPage popupPage = CreatePopupPage(viewModelType, parameter);

            if (Application.Current.MainPage is MasterDetailPage masterDetailPage)
            {
                await masterDetailPage.Navigation.PushPopupAsync(popupPage);
            }
        }

        public async Task RemoveLastFromBackStackAsync()
        {
            if (Application.Current.MainPage is NavigationPage mainPage)
            {
                var navigationStack = mainPage.Navigation.NavigationStack;
                var previousPage = navigationStack.ElementAtOrDefault(navigationStack.Count - 2);

                if (previousPage != null)
                {
                    mainPage.Navigation.RemovePage(previousPage);
                }
            }

            await Task.FromResult(true);
        }

        public async Task RemoveBackStackAsync()
        {
            if (Application.Current.MainPage is NavigationPage mainPage)
            {
                if (!mainPage.Navigation.NavigationStack.Any())
                {
                    return;
                }

                var lastPage = mainPage.Navigation.NavigationStack.Last();

                while (true)
                {
                    var pageToDelete = mainPage.Navigation.NavigationStack[0];

                    if (pageToDelete != null && pageToDelete != lastPage)
                    {
                        mainPage.Navigation.RemovePage(pageToDelete);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            await Task.FromResult(true);
        }

        public async Task NavigateBackAsync()
        {
            if (Application.Current.MainPage is MasterDetailPage detailPage)
            {
                await detailPage.Detail.Navigation.PopAsync(true);
            }
        }

        public async Task NavigatePopupBackAsync()
        {
            if (Application.Current.MainPage is MasterDetailPage detailPage)
            {
                await detailPage.Detail.Navigation.PopPopupAsync(true);
            }            
        }

        private async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreatePage(viewModelType, parameter);

            if (page.BindingContext is BaseViewModel viewModel)
            {
                await viewModel.InitializeAsync(parameter);
            }

            if (page is MenuView || page is LoginView || page is PinView)
            {
                Application.Current.MainPage = page;
            }

            else if (page is UserProfileView 
                     || page is NotesView 
                     || page is AchievementsView
                     || page is BirthdaysView 
                     || page is ToDoCalendarView 
                     || page is SettingsView)
            {
                if (Application.Current.MainPage is MasterDetailPage detailPage)
                {
                    detailPage.Detail = new NavigationPage(page);
                    await Task.Delay(25);
                    detailPage.IsPresented = false;
                }          
            }
            else
            {
                if (Application.Current.MainPage is MasterDetailPage detailPage)
                {
                    await detailPage.Detail.Navigation.PushAsync(page, true);
                    detailPage.IsPresented = false;
                }                               
            }
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.FullName?.Replace("Model", string.Empty);
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }

        private Page CreatePage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);

            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }

            try
            {
                Page page = Activator.CreateInstance(pageType) as Page;
                return page;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        private PopupPage CreatePopupPage(Type viewModelType, object parameter)
        {
            Type popupPageType = GetPageTypeForViewModel(viewModelType);

            if (popupPageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }

            try
            {
                var page = Activator.CreateInstance(popupPageType, args:parameter) as GalleryItemView;
                return page;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}