using ReactiveReorderingTest.DataModel;
using Realms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ReactiveReorderingTest.UWP.XAML
{
    /// <summary>
    /// This class is an experiment to a) figure out how to build a virualizing item source for a xaml listview without using incrementalloadingcollection b) figure out how realm collections work
    /// Not clear if it can be generalized to work with Xamarin forms or not hence the namespace
    /// It assumes that it is managing UpNextQueueEntry types which have an intrinsic sort order as a field of the type
    /// </summary>
    public class RealmVirtualOrderedUpNextEntrySource : INotifyCollectionChanged, IList, IItemsRangeInfo, ISelectionInfo
    {
        UpNextQueue r;
        private bool isReordering;
        private int removedOriginalIndex;
        private object removed;
        private int insertedNewIndex;
        private object inserted;

        public RealmVirtualOrderedUpNextEntrySource(UpNextQueue aRealm)
        {
            r = aRealm;

            var dispatcher = Window.Current.Dispatcher;

            var disposable = r.Queue.AsRealmCollection().SubscribeForNotifications((s,e,x) => {
                
                // TODO: need a more general impl
                if (e?.InsertedIndices?.Length == 1)
                {
                    dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => 
                    {
                        object changedItem = this[e.InsertedIndices[0]];
                        var what = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItem, e.InsertedIndices[0]);
                        CollectionChanged(this, what);
                    });
                }
            });

            //TODO implement idisposible and call the above
        }

        #region IItemsRangeInfo
        public void RangesChanged(ItemIndexRange visibleRange, IReadOnlyList<ItemIndexRange> trackedItems)
        {
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IList
        public object this[int index] {
            get => r.Queue.OrderBy(ob=>ob.QuePosition).ElementAt(index); set => throw new NotImplementedException();
        }

        public bool IsFixedSize => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public int Count => r.Queue.Count;

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
            return r.Queue.OrderBy(ob => ob.QuePosition).GetEnumerator();
        }

        public int IndexOf(object value)
        {
            return (int)((UpNextQueueEntry)value).QuePosition;
        }

        public void Insert(int index, object value)
        {
            insertedNewIndex = index;
            inserted = value;

            //var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new List<UpNextQueueEntry>() { f, s } as IList);
            //CollectionChanged(this, args);
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            removedOriginalIndex = index;
            removed = this[index];

            // TODO: this overload of remove currently on supports a move (i.e. remove followed by add).  The work is currently done in endreorder
            // TODO: detecting that it is a reorder must be handled by the caller (in this case the viewmodel for mainpageviewmodel)
            // TODO: it is possible / desirable that you would detect a reorder by spotting a remove followed by an add of the same item hence wouldn't need the caller to manage this
            // TODO: because this approach requires us to defer firing notifycollectionchanged until later (with move rather than remove, add) there is a slight noticable flicker when drag reordering which might be avoidable if firing remove then add at time of removal / add.  Should compare visually with listview, but this is good enough for now
            //var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new List<UpNextQueueEntry>() { f, s } as IList);
            //CollectionChanged(this, args);
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

        internal void BeginReorder()
        {
            removedOriginalIndex = -1;
            insertedNewIndex = -1;
            isReordering = true;
        }

        internal void EndReorder()
        {
            if (isReordering && removedOriginalIndex != -1 && insertedNewIndex != -1)
            {
                UpNextQueueEntry f = null;
                UpNextQueueEntry s = null;

                var tr = r.Realm.BeginWrite();
                f = r.Queue.Where(i => i.QuePosition == removedOriginalIndex).FirstOrDefault();
                s = r.Queue.Where(i => i.QuePosition == insertedNewIndex).FirstOrDefault();

                if (insertedNewIndex > removedOriginalIndex)
                {
                    f.QuePosition++;
                    s.QuePosition--;
                    tr.Commit();

                    var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, f, (int)f.QuePosition, (int)f.QuePosition - 1);
                    CollectionChanged(this, args);

                    args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, s, (int)s.QuePosition, (int)f.QuePosition + 1);
                    CollectionChanged(this, args);
                } else if (removedOriginalIndex > insertedNewIndex)
                {
                
                    f.QuePosition--;
                    s.QuePosition++;
                    tr.Commit();

                    var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, f, (int)f.QuePosition, (int)f.QuePosition + 1);
                    CollectionChanged(this, args);

                    args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, s, (int)s.QuePosition, (int)f.QuePosition - 1);
                    CollectionChanged(this, args);
                }
            }
            isReordering = false;
        }
    }
}
