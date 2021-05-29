using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;

using ReminderXamarin.ViewModels.Base;

using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ReminderXamarin.Services.Navigation
{
    [Preserve(AllMembers = true)]
    public class NavigationService : INavigationService
    {
        public Task ToRootAsync<TViewModel>() where TViewModel : BaseNavigableViewModel
        {
            return InternalNavigateToAsync(typeof(TViewModel), null, true);
        }

        public Task NavigateToDetails<TViewModel>(object parameter = null) where TViewModel : BaseNavigableViewModel
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter, false, true);
        }

        public Task NavigateToAsync<TViewModel>(object parameter = null) where TViewModel : BaseNavigableViewModel
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public Task NavigateToPopupAsync<TViewModel>(object parameter = null) where TViewModel : BaseNavigableViewModel
        {
            return InternalNavigateToPopupAsync(typeof(TViewModel), parameter);
        }

        private async Task InternalNavigateToPopupAsync(Type viewModelType, object parameter = null)
        {
            PopupPage popupPage = CreatePopupPage(viewModelType);

            if (popupPage.BindingContext is BaseNavigableViewModel viewModel)
            {
                await viewModel.InitializeAsync(parameter);
            }

            if (Application.Current.MainPage is MasterDetailPage masterDetailPage)
            {
                await masterDetailPage.Navigation.PushPopupAsync(popupPage);
            }
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

        private async Task InternalNavigateToAsync(Type viewModelType, 
            object parameter = null, 
            bool root = false, 
            bool isDetailChangeRequested = false)
        {
            Page page = CreatePage(viewModelType);

            if (page.BindingContext is BaseNavigableViewModel viewModel)
            {
                await viewModel.InitializeAsync(parameter);
            }

            if (root)
            {
                Application.Current.MainPage = page;
            }
            else if (isDetailChangeRequested)
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

        private Page CreatePage(Type viewModelType)
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

        private PopupPage CreatePopupPage(Type viewModelType)
        {
            Type popupPageType = GetPageTypeForViewModel(viewModelType);

            if (popupPageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }

            try
            {
                var page = Activator.CreateInstance(popupPageType) as PopupPage;
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