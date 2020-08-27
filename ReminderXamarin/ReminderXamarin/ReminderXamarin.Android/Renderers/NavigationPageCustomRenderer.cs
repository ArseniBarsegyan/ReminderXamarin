using Android.Content;

using ReminderXamarin.Droid.Renderers;
using ReminderXamarin.ViewModels;
using ReminderXamarin.Views;

using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationPageCustomRenderer))]
namespace ReminderXamarin.Droid.Renderers
{
    /// <summary>
    /// Override NavigationPageRenderer. Handle hardware and program "back" button press.
    /// If current page is CreateNotePage, ask user about leaving this page.
    /// </summary>
    public class NavigationPageCustomRenderer : NavigationPageRenderer
    {
        public NavigationPageCustomRenderer(Context context) : base(context)
        {
        }

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