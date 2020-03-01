using Xamarin.Forms;

namespace ReminderXamarin.Elements
{
    public class BindableToolbarItem : ToolbarItem
    {
        public static readonly BindableProperty IsVisibleProperty = 
            BindableProperty.Create(propertyName: nameof(IsVisible), 
                returnType: typeof(bool), 
                declaringType: typeof(BindableToolbarItem), 
                defaultValue: true, 
                defaultBindingMode: BindingMode.TwoWay, 
                propertyChanged: OnIsVisibleChanged);

        public bool IsVisible
        {
            get => (bool)GetValue(IsVisibleProperty);
            set => SetValue(IsVisibleProperty, value);
        }

        private static void OnIsVisibleChanged(BindableObject bindable, 
            object oldvalue, object newvalue)
        {
            var item = bindable as BindableToolbarItem;

            if (item == null || item.Parent == null)
            {
                return;
            }

            var toolbarItems = ((ContentPage)item.Parent).ToolbarItems;
            var isVisible = (bool)newvalue;

            if (isVisible && !toolbarItems.Contains(item))
            {
                Device.BeginInvokeOnMainThread(() => { toolbarItems.Add(item); });
            }
            else if (!isVisible && toolbarItems.Contains(item))
            {
                Device.BeginInvokeOnMainThread(() => { toolbarItems.Remove(item); });
            }
            else if (isVisible && toolbarItems.Contains(item))
            {
                Device.BeginInvokeOnMainThread(() => 
                { 
                    toolbarItems.Remove(item);
                    toolbarItems.Add(item);
                });
            }
        }
    }
}
