using ReminderXamarin.Extensions;
using ReminderXamarin.ViewModels;
using Xamarin.Forms;

namespace ReminderXamarin.Helpers
{
    public class NotesListDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ImageCellTemplate { get; set; }
        public DataTemplate TextCellTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is NoteViewModel viewModel)
            {
                if (!viewModel.GalleryItemsViewModels.IsNullOrEmpty())
                {
                    return ImageCellTemplate;
                }
            }
            return TextCellTemplate;
        }
    }
}