﻿using System;
using System.ComponentModel;
using Android.Content;
using Android.Content.Res;
using ReminderXamarin.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using FAB = Android.Support.Design.Widget.FloatingActionButton;

[assembly: ExportRenderer(typeof(ReminderXamarin.Elements.FloatingActionButton), typeof(FloatingActionButtonRenderer))]
namespace ReminderXamarin.Droid.Renderers
{
    public class FloatingActionButtonRenderer : Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<Elements.FloatingActionButton, FAB>
    {
        public FloatingActionButtonRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Elements.FloatingActionButton> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
            {
                return;
            }

            if (Element == null)
            {
                return;
            }

            var fab = new FAB(Context);
            fab.BackgroundTintList = ColorStateList.ValueOf(Element.ButtonColor.ToAndroid());

            var elementImage = Element.Image;
            var imageFile = elementImage?.File;

            if (imageFile != null)
            {
                fab.SetImageDrawable(Context.GetDrawable(imageFile));
            }
            fab.Click += Fab_Click;
            SetNativeControl(fab);

        }
        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);
            Control.BringToFront();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var fab = (FAB)Control;
            if (e.PropertyName == nameof(Element.ButtonColor))
            {
                fab.BackgroundTintList = ColorStateList.ValueOf(Element.ButtonColor.ToAndroid());
            }
            if (e.PropertyName == nameof(Element.Image))
            {
                var elementImage = Element.Image;
                var imageFile = elementImage?.File;

                if (imageFile != null)
                {
                    fab.SetImageDrawable(Context.GetDrawable(imageFile));
                }
            }
            base.OnElementPropertyChanged(sender, e);

        }

        private void Fab_Click(object sender, EventArgs e)
        {
            ((IButtonController)Element).SendClicked();
        }
    }
}