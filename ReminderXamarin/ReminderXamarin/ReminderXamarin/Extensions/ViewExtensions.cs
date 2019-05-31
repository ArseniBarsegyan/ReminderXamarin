using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ReminderXamarin.Extensions
{
    public static class ViewExtensions
    {
        public static Task<bool> ColorTo(this VisualElement element, Color fromColor, Color toColor, Action<Color> callback,
            uint length = 250, Easing easing = null)
        {
            Func<double, Color> transform = t =>
                Color.FromRgba(fromColor.R + t * (toColor.R - fromColor.R),
                    fromColor.G + t * (toColor.G - fromColor.G),
                    fromColor.B + t * (toColor.B - fromColor.B),
                    fromColor.A + t * (toColor.A - fromColor.A));

            return ColorAnimation(element, nameof(ColorTo), transform, callback, length, easing);
        }

        public static void CancelAnimation(this VisualElement element)
        {
            element.AbortAnimation(nameof(ColorTo));
        }

        private static Task<bool> ColorAnimation(VisualElement element, string name, Func<double, Color> transform, Action<Color> callback, uint length, Easing easing)
        {
            easing = easing ?? Easing.Linear;
            var taskCompletionSource = new TaskCompletionSource<bool>();
            element.Animate(name, transform, callback, 16, length, easing, (v, c) => taskCompletionSource.SetResult(c));
            return taskCompletionSource.Task;
        }
    }
}