using System;
using Foundation;
using ReminderXamarin.Elements;
using ReminderXamarin.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomCollectionView), typeof(CustomCollectionViewRenderer))]
namespace ReminderXamarin.iOS.Renderers
{
    public class CustomCollectionViewRenderer : CollectionViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<GroupableItemsView> e)
        {
            base.OnElementChanged(e);

            if (Element is CustomCollectionView formsCollectionView)
            {
                if(Control?.PreferredFocusEnvironments[0] is UICollectionView collectionView)
                {
                    collectionView.Delegate = new CustomCollectionViewDelegate(
                        new WeakReference<CustomCollectionView>(formsCollectionView));
                }
            }
        }
    }

    public class CustomCollectionViewDelegate : UICollectionViewDelegate
    {
        private readonly WeakReference<CustomCollectionView> _customCollectionView;

        public CustomCollectionViewDelegate(WeakReference<CustomCollectionView> customCollectionView)
        {
            _customCollectionView = customCollectionView;
        }

        public override void WillDisplayCell(UICollectionView collectionView, UICollectionViewCell cell, NSIndexPath indexPath)
        {
            if (_customCollectionView.TryGetTarget(out var formsCollectionView))
            {
                formsCollectionView.RaiseAppeared(indexPath.Row);
            }
        }

        public override void CellDisplayingEnded(UICollectionView collectionView, UICollectionViewCell cell, NSIndexPath indexPath)
        {
            if (_customCollectionView.TryGetTarget(out var formsCollectionView))
            {
                formsCollectionView.RaiseDisappeared(indexPath.Row);
            }
        }
    }
}