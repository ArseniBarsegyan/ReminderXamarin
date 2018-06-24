using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace ReminderXamarin.Elements
{
    /// <inheritdoc />
    /// <summary>
    /// <see cref="ImageGallery"/> extends absolute layout and takes collection of <see cref="Image"/> in
    /// constructor as parameter. This class uses <see cref="CarouselView"/> to draw horizontal set of images
    /// that could be changed by swipe.
    /// </summary>
    public class ImageGallery : AbsoluteLayout
    {
        private readonly CarouselView _carousel;

        public ImageGallery(ObservableCollection<Image> images)
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;

            _carousel = new CarouselView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HeightRequest = 350
            };

            Images = images;
            _carousel.ItemsSource = images;
            _carousel.PositionSelected += OnImageChanged;

            Children.Add(_carousel);
            SetLayoutBounds(_carousel, new Rectangle(0, 0, 1, 1));
            SetLayoutFlags(_carousel, AbsoluteLayoutFlags.All);
        }

        /// <summary>
        /// Collection of <seealso cref="Image"/> as item source for image gallery.
        /// </summary>
        public ObservableCollection<Image> Images
        {
            get => (ObservableCollection<Image>)_carousel.ItemsSource;
            set => _carousel.ItemsSource = value;
        }

        /// <summary>
        /// Item template for image gallery.
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get => _carousel.ItemTemplate;
            set => _carousel.ItemTemplate = value;
        }

        public event EventHandler<int> ImageChanged;

        /// <summary>
        /// Display current image gallery element by number.
        /// </summary>
        /// <param name="position">number of element to be displayed.</param>
        public void SetCurrentPosition(int position)
        {
            _carousel.Position = position;
        }

        private void OnImageChanged(object sender, SelectedPositionChangedEventArgs e)
        {
            // Get the selected page
            var position = (int)e.SelectedPosition;
            ImageChanged?.Invoke(this, position);
        }
    }
}