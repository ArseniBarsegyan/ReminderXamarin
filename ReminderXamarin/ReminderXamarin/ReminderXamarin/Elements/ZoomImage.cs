using System;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ReminderXamarin.Elements
{
    public class ZoomImage : ContentView
    {
        private double _currentScale = 1;
        private double _startScale = 1;
        private double _xOffset = 0;
        private double _yOffset = 0;
        private bool _isZoomed;

        public ZoomImage()
        {
            PinchGestureRecognizer pinchGesture = new PinchGestureRecognizer();
            pinchGesture.PinchUpdated += PinchUpdated;
            GestureRecognizers.Add(pinchGesture);

            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            GestureRecognizers.Add(panGesture);

            var tapGesture = new TapGestureRecognizer { NumberOfTapsRequired = 2 };
            tapGesture.Tapped += DoubleTapped;
            GestureRecognizers.Add(tapGesture);
        }

        private void PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            _isZoomed = true;

            if (e.Status == GestureStatus.Started)
            {
                _startScale = Content.Scale;
                Content.AnchorX = 0;
                Content.AnchorY = 0;
            }

            if (e.Status == GestureStatus.Running)
            {
                double scale = _currentScale + (e.Scale - 1) * _startScale;
                _currentScale = Math.Max(1, scale);

                double renderedX = Content.X + _xOffset;
                double deltaX = renderedX / Width;
                double deltaWidth = Width / (Content.Width * _startScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                double renderedY = Content.Y + _yOffset;
                double deltaY = renderedY / Height;
                double deltaHeight = Height / (Content.Height * _startScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                double targetX = _xOffset - (originX * Content.Width) * (_currentScale - _startScale);
                double targetY = _yOffset - (originY * Content.Height) * (_currentScale - _startScale);

                Content.TranslationX = Math.Min(0, Math.Max(targetX, -Content.Width * (_currentScale - 1)));
                Content.TranslationY = Math.Min(0, Math.Max(targetY, -Content.Height * (_currentScale - 1)));

                Content.Scale = _currentScale;
            }

            if (e.Status == GestureStatus.Completed)
            {
                _xOffset = Content.TranslationX;
                _yOffset = Content.TranslationY;
            }

            if (Content.Scale == 1)
            {
                _isZoomed = false;
            }
        }

        public void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (Content.Scale == 1)
            {
                return;
            }

            switch (e.StatusType)
            {
                case GestureStatus.Running:

                    double newX = (e.TotalX * Scale) + _xOffset;
                    double newY = (e.TotalY * Scale) + _yOffset;

                    double width = (Content.Width * Content.Scale);
                    double height = (Content.Height * Content.Scale);

                    bool canMoveX = width > Application.Current.MainPage.Width;
                    bool canMoveY = height > Application.Current.MainPage.Height;

                    if (canMoveX)
                    {
                        double minX = (width - (Application.Current.MainPage.Width / 2)) * -1;
                        double maxX = Math.Min(Application.Current.MainPage.Width / 2, width / 2);

                        if (newX < minX)
                        {
                            newX = minX;
                        }

                        if (newX > maxX)
                        {
                            newX = maxX;
                        }
                    }
                    else
                    {
                        newX = 0;
                    }

                    if (canMoveY)
                    {
                        double minY = (height - (Application.Current.MainPage.Height / 2)) * -1;
                        double maxY = Math.Min(Application.Current.MainPage.Width / 2, height / 2);

                        if (newY < minY)
                        {
                            newY = minY;
                        }

                        if (newY > maxY)
                        {
                            newY = maxY;
                        }
                    }
                    else
                    {
                        newY = 0;
                    }

                    Content.TranslationX = newX;
                    Content.TranslationY = newY;
                    break;
                case GestureStatus.Completed:
                    _xOffset = Content.TranslationX;
                    _yOffset = Content.TranslationY;
                    break;
            }
        }

        public async void DoubleTapped(object sender, EventArgs e)
        {
            _startScale = Content.Scale;
            Content.AnchorX = 0;
            Content.AnchorY = 0;

            _isZoomed = !_isZoomed;
            await Zoom(_isZoomed);

            _xOffset = Content.TranslationX;
            _yOffset = Content.TranslationY;
        }

        private async Task Zoom(bool zoomIn)
        {
            double multiplicator = Math.Pow(2, 1.0 / 10.0);

            for (int i = 0; i < 10; i++)
            {
                if (zoomIn)
                {
                    _currentScale *= multiplicator;
                }
                else
                {
                    _currentScale /= multiplicator;
                }
                
                double renderedX = Content.X + _xOffset;
                double deltaX = renderedX / Width;
                double deltaWidth = Width / (Content.Width * _startScale);
                double originX = (0.5 - deltaX) * deltaWidth;

                double renderedY = Content.Y + _yOffset;
                double deltaY = renderedY / Height;
                double deltaHeight = Height / (Content.Height * _startScale);
                double originY = (0.5 - deltaY) * deltaHeight;

                double targetX = _xOffset - (originX * Content.Width) * (_currentScale - _startScale);
                double targetY = _yOffset - (originY * Content.Height) * (_currentScale - _startScale);

                Content.TranslationX = Math.Min(0, Math.Max(targetX, -Content.Width * (_currentScale - 1)));
                Content.TranslationY = Math.Min(0, Math.Max(targetY, -Content.Height * (_currentScale - 1)));

                Content.Scale = _currentScale;
                await Task.Delay(10);
            }
        }
    }
}