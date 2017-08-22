using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using ReactiveReorderingTest.DataModel;
using ReactiveReorderingTest.Services;
using ReactiveReorderingTest.UWP.XAML;
using Realms;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace ReactiveReorderingTest.ViewModels
{
    public class MainPageViewmodel : ViewModelBase
    {
        Realm r = null;

        public UpNextQueue UpNext { get; private set; }

        public RealmVirtualOrderedUpNextEntrySource VirtualItemSource { get; private set; }

        public RealmAllEpisodeSource VirtualAllEpisodeSource { get; private set; }

        // This is used to test how observablecollection implements INotifyCollectionChanged, System.Collections.IList, IItemsRangeInfo, ISelectionInfo
        //public ObservableCollection<UpNextQueueEntry> NonVirtualItemSource { get; private set; }

        public MainPageViewmodel()
        {
            r = Realm.GetInstance("testdb");

            var queue = r.All<UpNextQueue>();

            if (queue.Count() == 0)
            {
                CreateInitialData();
            }
            else
            {
                this.UpNext = queue.FirstOrDefault();
                this.UpNext.CurrentEpisode.PlaybackState.PropertyChanged += (o, ex) => {
                    Debug.WriteLine("State changed");
                    RaisePropertyChanged("UpNext");
                };

                this.UpNext.CurrentEpisode.PropertyChanged += (o, ex) => {
                    Debug.WriteLine("CurrentEpisode changed");
                };

                this.UpNext.PropertyChanged += (o, ex) => {
                    Debug.WriteLine("Upnext changed");
                };

                RaisePropertyChanged("UpNext");
            }

            VirtualItemSource = new RealmVirtualOrderedUpNextEntrySource(this.UpNext);

            VirtualAllEpisodeSource = new RealmAllEpisodeSource();

            #region fortesting
            //used to test observablecolleciton behavior
            //NonVirtualItemSource = new ObservableCollection<UpNextQueueEntry>();
            //NonVirtualItemSource.CollectionChanged += NonVirtualItemSource_CollectionChanged;

            //foreach (var item in VirtualItemSource)
            //{
            //    NonVirtualItemSource.Add((UpNextQueueEntry)item);
            //} 
            #endregion

            PlaybackCommand = new RelayCommand(() => {
                var service = SimpleIoc.Default.GetInstance<IFakeMediaplayerService>();
                service.Play();
            });

            StopCommand = new RelayCommand(() => {
                var service = SimpleIoc.Default.GetInstance<IFakeMediaplayerService>();
                service.Stop();
            });
        }

        //private void NonVirtualItemSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    Debug.WriteLine(sender, e.Action.ToString());
        //}

        internal void EndReorder()
        {
            VirtualItemSource.EndReorder();
        }

        internal void BeginReorder()
        {
            VirtualItemSource.BeginReorder();
        }

        public ICommand PlaybackCommand { get; private set; }

        public ICommand StopCommand { get; private set; }

        private void CreateInitialData()
        {
            var trans = r.BeginWrite();

            Feed f = new Feed() { Title = "Windows Weekly", Uri = new Uri("http://feeds.twit.tv/ww.xml").ToString() };
            r.Add(f);

            Episode e = new Episode() { Title = "WW 531: B is for Broadwell", Feed = f, Descrtiption = "The one where Paul misbehaves", UriKey = "http://www.podtrac.com/pts/redirect.mp3/cdn.twit.tv/audio/ww/ww0531/ww0531.mp3" };
            e.PlaybackState = new PlaybackState() { ElapsedTime = 10 };
            r.Add(e);

            UpNextQueue u = new UpNextQueue() { CurrentEpisode = e };
            r.Add(u);

            UpNextQueueEntry en = new UpNextQueueEntry() { Episode = e, QuePosition = 0 };
            u.Queue.Insert(0, en);

            f = new Feed() { Title = "MacBreak Weekly", Uri = new Uri("http://feeds.twit.tv/mbw.xml").ToString() };
            r.Add(f);

            e = new Episode() { Title = "MBW 571: Cardboard Hats for Dung Beetles", Feed = f, Descrtiption = "The one where Paul misbehaves", UriKey = "http://www.podtrac.com/pts/redirect.mp3/cdn.twit.tv/audio/mbw/mbw0571/mbw0571.mp3" };
            r.Add(e);
            en = new UpNextQueueEntry() { Episode = e, QuePosition = 1 };
            u.Queue.Insert(1, en);

            trans.Commit();
        }
    }
}
