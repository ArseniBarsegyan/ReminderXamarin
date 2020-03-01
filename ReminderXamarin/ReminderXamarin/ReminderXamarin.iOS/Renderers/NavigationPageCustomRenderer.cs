using ReminderXamarin.iOS.Renderers;
using ReminderXamarin.ViewModels;
using ReminderXamarin.Views;

using Rm.Helpers;

using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationPageCustomRenderer))]
namespace ReminderXamarin.iOS.Renderers
{
    /// <summary>
    /// Override NavigationPageRenderer. Handle hardware and program "back" button press.
    /// If current page is CreateNotePage, ask user about leaving this page.
    /// </summary>
    public class NavigationPageCustomRenderer : NavigationRenderer
    {
        protected override async Task<bool> OnPopViewAsync(Page page, bool animated)
        {
            if (page.Navigation.NavigationStack.LastOrDefault() is NoteEditView noteEditView)
            {
                if (noteEditView.BindingContext is NoteEditViewModel viewModel)
                {
                    if (viewModel.ShouldPromptUser)
                    {
                        bool result = await viewModel.AskAboutLeave();

                        if (result)
                        {
                            return await base.OnPopViewAsync(page, animated);
                        }
                        return await Task.FromResult(false);
                    }
                }                
            }
            return await base.OnPopViewAsync(page, animated);
        }
    }
}