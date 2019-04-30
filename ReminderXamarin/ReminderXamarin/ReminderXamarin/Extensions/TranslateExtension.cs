using System;
using System.Reflection;
using System.Resources;
using Plugin.Multilingual;
using Rm.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReminderXamarin.Extensions
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        private const string ResourceId = ConstantsHelper.TranslationResourcePath;
        private static readonly Lazy<ResourceManager> ResourceManager = new Lazy<ResourceManager>(() => 
            new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly));

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
            {
                return string.Empty;
            }
            var ci = CrossMultilingual.Current.CurrentCultureInfo;
            var translation = ResourceManager.Value.GetString(Text, ci) ?? Text;
            return translation;
        }
    }
}