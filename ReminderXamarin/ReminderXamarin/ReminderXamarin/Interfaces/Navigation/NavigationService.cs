using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ReminderXamarin.ViewModels.Base;
using Xamarin.Forms;

namespace ReminderXamarin.Interfaces.Navigation
{
    public class NavigationService : INavigationService
    {
        private Type _rootViewModelType;

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
            _rootViewModelType = typeof(TViewModel);
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
                    return;

                _rootViewModelType = mainPage.CurrentPage.BindingContext?.GetType();
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
            if (Application.Current.MainPage is NavigationPage mainPage)
            {
                await mainPage.PopAsync();
            }
        }


        private async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreatePage(viewModelType, parameter);

            if (page.BindingContext is BaseViewModel viewModel)
            {
                await viewModel.InitializeAsync(parameter);
            }

            if (_rootViewModelType == viewModelType)
            {
                Application.Current.MainPage = page;
            }
            else
            {
                if (Application.Current.MainPage is NavigationPage navigationPage)
                {
                    if (navigationPage.Navigation.NavigationStack.Last().GetType() == page.GetType()) return;
                    await navigationPage.PushAsync(page);
                }
                else
                {
                    Application.Current.MainPage = new NavigationPage(page);
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

            Page page = Activator.CreateInstance(pageType) as Page;
            return page;
        }
    }
}