using System;
using Xamarin.Forms;

namespace ReminderXamarin.Elements
{
    public class CustomCollectionView : CollectionView
    {
        public event EventHandler<int> ItemAppeared;
        public event EventHandler<int> ItemDisappeared;
        
        public void RaiseAppeared(int index)
        {
            ItemAppeared?.Invoke(this, index);
        }

        public void RaiseDisappeared(int index)
        {
            ItemDisappeared?.Invoke(this, index);
        }
    }
}