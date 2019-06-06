using System;
using Xamarin.Forms;

namespace ReminderXamarin.Elements
{
    /// <inheritdoc />
    /// <summary>
    /// Image with ability to zoom and pan.
    /// </summary>
    public class ZoomImage : Image
    {
        private double _currentScale = 1;
        private double _startScale = 1;
        private double _maxScale = 3;

        public ZoomImage()
        {
            var pinch = new PinchGestureRecognizer();
            pinch.PinchUpdated += PinchGestureRecognizer_PinchUpdated;
            GestureRecognizers.Add(pinch);

            var pan = new PanGestureRecognizer();
            pan.PanUpdated += PanGestureRecognizer_PanUpdated;
            GestureRecognizers.Add(pan);

            var tap = new TapGestureRecognizer { NumberOfTapsRequired = 2 };
            tap.Tapped += TapGestureRecognizer_Tapped;
            GestureRecognizers.Add(tap);

            Scale = _startScale;
            TranslationX = TranslationY = 0;
            AnchorX = AnchorY = 0;
        }

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (e.StatusType == GestureStatus.Running)
            {
                this.TranslateTo(e.TotalX * _currentScale, e.TotalY * _currentScale, 70, Easing.Linear);
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (_currentScale < _maxScale)
            {
                _currentScale = _maxScale;
                this.ScaleTo(_currentScale, 70, Easing.CubicIn);
            }
            else
            {
                _currentScale = _startScale;
                this.ScaleTo(_currentScale, 70, Easing.CubicOut);
            }
        }

        private void PinchGestureRecognizer_PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Started)
            {
                AnchorX = e.ScaleOrigin.X;
                AnchorY = e.ScaleOrigin.Y;
            }
            if (e.Status == GestureStatus.Running)
            {
            }
            if (e.Status == GestureStatus.Completed)
            {
                _currentScale = e.Scale;
                if (_currentScale > _maxScale)
                {
                    _currentScale = _maxScale;
                }
                if (_currentScale < _startScale)
                {
                    _currentScale = _startScale;
                }
                this.RelScaleTo(Scale * _currentScale, 70, Easing.Linear);
            }
        }
    }
}