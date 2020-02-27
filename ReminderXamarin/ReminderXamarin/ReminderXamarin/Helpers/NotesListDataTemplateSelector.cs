using ReminderXamarin.Extensions;

using Rm.Data.Data.Entities;

using Xamarin.Forms;

namespace ReminderXamarin.Helpers
{
    public class NotesListDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ImageCellTemplate { get; set; }
        public DataTemplate TextCellTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is Note model)
            {
                if (!model.GalleryItems.IsNullOrEmpty())
                {
                    return ImageCellTemplate;
                }
            }
            return TextCellTemplate;
        }
    }
}