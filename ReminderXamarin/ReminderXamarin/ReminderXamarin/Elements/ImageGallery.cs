using System.Collections;
using Xamarin.Forms;

namespace ReminderXamarin.Elements
{
    /// <summary>
    /// Scroll view that accepts images as ItemsSource and display them.
    /// </summary>
    public class ImageGallery : ScrollView
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(ImageGallery), default(IEnumerable));

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(ImageGallery), default(DataTemplate));

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        /// <summary>
        /// Call this method in custom renderer in Android and iOS projects.
        /// </summary>
        public void Render()
        {
            if (ItemTemplate == null || ItemsSource == null)
            {
                return;
            }

            var layout = new StackLayout
            {
                Orientation = Orientation == ScrollOrientation.Vertical
                    ? StackOrientation.Vertical
                    : StackOrientation.Horizontal
            };

            foreach (var item in ItemsSource)
            {
                if (ItemTemplate.CreateContent() is ViewCell viewCell)
                {
                    viewCell.View.BindingContext = item;
                    layout.Children.Add(viewCell.View);
                }
            }

            Content = layout;
        }
    }
}