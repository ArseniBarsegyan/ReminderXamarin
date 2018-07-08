using System.Linq;
using System.Threading.Tasks;
using ReminderXamarin.Helpers;
using ReminderXamarin.iOS.Renderers;
using ReminderXamarin.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationPageCustomRenderer))]
namespace ReminderXamarin.iOS.Renderers
{
    /// <summary>
    /// Overrided <see cref="NavigationRenderer"/>. Handle hardware and program "back" button press.
    /// If current page is <see cref="CreateNotePage"/>, ask user about leaving this page.
    /// </summary>
    public class NavigationPageCustomRenderer : NavigationRenderer
    {
        protected override async Task<bool> OnPopViewAsync(Page page, bool animated)
        {
            if (page.Navigation.NavigationStack.LastOrDefault() is CreateNotePage createNotePage)
            {
                bool result = await createNotePage.DisplayAlert(ConstantHelper.Warning,
                    ConstantHelper.PageCloseMessage, ConstantHelper.Ok, ConstantHelper.Cancel);

                if (result)
                {
                    return await base.OnPopViewAsync(page, animated);
                }
                return await Task.FromResult(false);
            }
            return await base.OnPopViewAsync(page, animated);
        }
    }
}