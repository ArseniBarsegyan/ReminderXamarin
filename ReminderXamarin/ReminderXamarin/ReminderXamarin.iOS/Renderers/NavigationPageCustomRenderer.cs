﻿using System.Linq;
using System.Threading.Tasks;
using Rm.Helpers;
using ReminderXamarin.iOS.Renderers;
using ReminderXamarin.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationPageCustomRenderer))]
namespace ReminderXamarin.iOS.Renderers
{
    /// <summary>
    /// Overrided <see cref="NavigationRenderer"/>. Handle hardware and program "back" button press.
    /// If current page is <see cref="NoteCreateView"/>, ask user about leaving this page.
    /// </summary>
    public class NavigationPageCustomRenderer : NavigationRenderer
    {
        protected override async Task<bool> OnPopViewAsync(Page page, bool animated)
        {
            if (page.Navigation.NavigationStack.LastOrDefault() is NoteCreateView noteCreatePage)
            {
                if (noteCreatePage.ShouldDisplayMessage())
                {
                    bool result = await noteCreatePage.DisplayAlert(ConstantsHelper.Warning,
                        ConstantsHelper.PageCloseMessage, ConstantsHelper.Ok, ConstantsHelper.Cancel);

                    if (result)
                    {
                        return await base.OnPopViewAsync(page, animated);
                    }
                    return await Task.FromResult(false);
                }
            }
            return await base.OnPopViewAsync(page, animated);
        }
    }
}