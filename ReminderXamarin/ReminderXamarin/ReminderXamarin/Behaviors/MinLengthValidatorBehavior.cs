using Xamarin.Forms;

namespace ReminderXamarin.Behaviors
{
    public class MinLengthValidatorBehavior : Behavior<Entry>
    {
        private static readonly BindablePropertyKey IsValidPropertyKey = 
            BindableProperty.CreateReadOnly(
                propertyName: nameof(IsValid), 
                returnType: typeof(bool), 
                declaringType: typeof(MinLengthValidatorBehavior), 
                defaultValue: false);
        
        private static readonly BindableProperty ValidNumberTextColorProperty = 
            BindableProperty.Create(
                propertyName: nameof(ValidNumberTextColor),
                returnType: typeof(Color),
                declaringType: typeof(MinLengthValidatorBehavior),
                defaultValue: (Color)Application.Current.Resources["TextCommon"]);
        
        private static readonly BindableProperty InvalidNumberTextColorProperty = 
            BindableProperty.Create(
                propertyName: nameof(InvalidNumberTextColor),
                returnType: typeof(Color),
                declaringType: typeof(MinLengthValidatorBehavior),
                defaultValue: (Color)Application.Current.Resources["ErrorTextCommon"]);
        
        public static readonly BindableProperty MinLengthProperty = 
            BindableProperty.Create(
                propertyName: nameof(MinLength),
                returnType: typeof(int),
                declaringType: typeof(MinLengthValidatorBehavior),
                defaultValue: 1);
        
        public static readonly BindableProperty IsValidProperty = 
            IsValidPropertyKey.BindableProperty;

        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            private set => SetValue(IsValidPropertyKey, value);
        }

        public Color ValidNumberTextColor
        {
            get => (Color)GetValue(ValidNumberTextColorProperty);
            set => SetValue(ValidNumberTextColorProperty, value);
        }

        public Color InvalidNumberTextColor
        {
            get => (Color)GetValue(InvalidNumberTextColorProperty);
            set => SetValue(InvalidNumberTextColorProperty, value);
        }

        public int MinLength
        {
            get => (int)GetValue(MinLengthProperty);
            set => SetValue(MinLengthProperty, value);
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += Bindable_TextChanged;
        }

        private void Bindable_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length > 0 && e.NewTextValue.Length >= MinLength)
            {
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }
            ((Entry)sender).TextColor = IsValid ? 
                ValidNumberTextColor 
                : InvalidNumberTextColor;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= Bindable_TextChanged;
        }
    }
}
