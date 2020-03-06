using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

using MvvmHelpers;

namespace ReminderXamarin.Collections
{
    public class RangeObservableCollection<T> : ObservableRangeCollection<T>
    {
        public void ReplaceRangeWithoutUpdating(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            Items.Clear();

            foreach (T item in collection)
            {
                Items.Add(item);
            }
        }

        public void RaiseCollectionChanged(NotifyCollectionChangedAction mode = NotifyCollectionChangedAction.Reset)
        {
            if (mode == NotifyCollectionChangedAction.Reset)
            {
                OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
    }
}
