using ReminderXamarin.Elements;
using System;
using Xamarin.Forms;

namespace ReminderXamarin.Behaviors
{
    /// <inheritdoc />
    /// <summary>
    /// Provide behavior to EditorWithPlaceholder control.
    /// <para />
    /// Attach this behavior to EditorWithPlaceholder control in XAML.
    /// </summary>
    public class EditorWithPlaceholderBehavior : Behavior<EditorWithBorder>
    {
        protected override void OnAttachedTo(EditorWithBorder editor)
        {
            editor.BindingContextChanged += OnBindingContextChanged;
            editor.Focused += OnEditorFocused;
            editor.Unfocused += OnEditorUnFocused;
            base.OnAttachedTo(editor);

            if (string.IsNullOrEmpty(editor.Text))
            {
                editor.Text = editor.Placeholder;
                editor.TextColor = editor.PlaceholderColor;
            }
        }

        protected override void OnDetachingFrom(EditorWithBorder editor)
        {
            editor.BindingContextChanged -= OnBindingContextChanged;
            editor.Focused -= OnEditorFocused;
            editor.Unfocused -= OnEditorUnFocused;
            base.OnDetachingFrom(editor);
        }

        private void OnBindingContextChanged(object sender, EventArgs args)
        {
            var editor = sender as EditorWithBorder;
            if (string.IsNullOrEmpty(editor.Text))
            {
                editor.Text = editor.Placeholder;
                editor.TextColor = editor.PlaceholderColor;
            }
        }

        private void OnEditorFocused(object sender, FocusEventArgs args)
        {
            var editor = sender as EditorWithBorder;
            string placeholder = editor.Placeholder;
            string text = editor.Text;

            if (placeholder == text)
            {
                editor.Text = string.Empty;
            }
            editor.TextColor = Color.Default;
        }

        private void OnEditorUnFocused(object sender, FocusEventArgs args)
        {
            var editor = sender as EditorWithBorder;
            string placeholder = editor.Placeholder;
            string text = editor.Text;

            if (string.IsNullOrEmpty(text))
            {
                editor.Text = placeholder;
                editor.TextColor = editor.PlaceholderColor;
            }
            else
            {
                editor.TextColor = Color.Default;
            }
        }
    }
}
