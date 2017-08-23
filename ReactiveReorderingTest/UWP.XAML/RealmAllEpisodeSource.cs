using ReactiveReorderingTest.DataModel;
using Realms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace ReactiveReorderingTest.UWP.XAML
{
    public class RealmAllEpisodeSource : INotifyCollectionChanged, IList, IItemsRangeInfo, ISelectionInfo
    {
        Realm r;
        IQueryable<Episode> query;

        public RealmAllEpisodeSource()
        {
            r = Realm.GetInstance("testdb");
            query = r.All<DataModel.Episode>().OrderByDescending(ob => ob.PublishedDate);
        }

        #region IItemsRangeInfo
        public void RangesChanged(ItemIndexRange visibleRange, IReadOnlyList<ItemIndexRange> trackedItems)
        {
            Debug.WriteLine($"{visibleRange.FirstIndex} + {visibleRange.LastIndex}");
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion

        List<Episode> cache = null;

        #region IList
        public object this[int index]
        {
            get
            {
                return query.ElementAt(index);

                // we really need skip / take.  The following code proves that read perf is an issue

                if (cache == null)
                {
                    //cache = query.Take(200).ToList();
                    cache = query.ToList();
                }
                return cache[index];
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsFixedSize => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public int Count => query.Count();

        public bool IsSynchronized => throw new NotImplementedException();

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public object SyncRoot => throw new NotImplementedException();
        #endregion

        #region INotifyCollectionChanged
        public event NotifyCollectionChangedEventHandler CollectionChanged;


        #endregion

        #region ISelectionInfo
        public void SelectRange(ItemIndexRange itemIndexRange)
        {
            throw new NotImplementedException();
        }

        public void DeselectRange(ItemIndexRange itemIndexRange)
        {
            throw new NotImplementedException();
        }

        public bool IsSelected(int index)
        {
            return false;
        }

        public IReadOnlyList<ItemIndexRange> GetSelectedRanges()
        {
            return null;
        }
        #endregion 
    }
}
