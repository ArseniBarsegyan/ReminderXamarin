using Android.Content;
using AndroidX.RecyclerView.Widget;
using ReminderXamarin.Droid.Renderers;
using ReminderXamarin.Elements;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomCollectionView), typeof(CustomCollectionViewRenderer))]
namespace ReminderXamarin.Droid.Renderers
{
    public class CustomCollectionViewRenderer : CollectionViewRenderer
    {
        private CustomCollectionView _collectionView;
        
        public CustomCollectionViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ItemsView> e)
        {
            base.OnElementChanged(e);

            if (Element is CustomCollectionView collectionView)
            {
                _collectionView = collectionView;
            }
            
            if (View is RecyclerView recyclerView)
            {
                recyclerView.ChildViewAttachedToWindow += RecyclerViewOnChildViewAttachedToWindow;
                recyclerView.ChildViewDetachedFromWindow += RecyclerViewOnChildViewDetachedFromWindow;
            }
        }

        private void RecyclerViewOnChildViewAttachedToWindow(object sender, ChildViewAttachedToWindowEventArgs e)
        {
            if (!(sender is RecyclerView recyclerView)) 
                return;
            
            var viewHolder = recyclerView.GetChildViewHolder(e.View);
            var position = viewHolder.AdapterPosition;
            _collectionView?.RaiseAppeared(position);
        }
        
        private void RecyclerViewOnChildViewDetachedFromWindow(object sender, ChildViewDetachedFromWindowEventArgs e)
        {
            if (!(sender is RecyclerView recyclerView)) 
                return;
            
            var viewHolder = recyclerView.GetChildViewHolder(e.View);
            var position = viewHolder.AdapterPosition;
            _collectionView?.RaiseDisappeared(position);
        }
    }
}